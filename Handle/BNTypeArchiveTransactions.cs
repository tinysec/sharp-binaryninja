using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Performs archive mutations in a new snapshot transaction.
	/// </summary>
	/// <param name="snapshotId">Identifier reserved for the new snapshot.</param>
	public delegate void TypeArchiveSnapshotTransaction(string snapshotId);

	public sealed partial class TypeArchive
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool NativeSnapshotTransaction(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string snapshotId);

		private sealed class SnapshotTransactionContext
		{
			private readonly TypeArchiveSnapshotTransaction transaction;
			private ExceptionDispatchInfo? exception;

			internal SnapshotTransactionContext(TypeArchiveSnapshotTransaction transaction)
			{
				this.transaction = transaction;
			}

			internal bool Invoke(IntPtr context, string snapshotId)
			{
				try
				{
					this.transaction(snapshotId);
					return true;
				}
				catch (Exception caught)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
					return false;
				}
			}

			internal void ThrowIfFailed()
			{
				if (null != this.exception)
				{
					this.exception.Throw();
				}
			}
		}

		private sealed class MergeProgressContext
		{
			private readonly ProgressDelegate? progress;
			private ExceptionDispatchInfo? exception;

			internal MergeProgressContext(ProgressDelegate? progress)
			{
				this.progress = progress;
			}

			internal bool Invoke(IntPtr context, ulong current, ulong total)
			{
				if (null == this.progress)
				{
					return true;
				}

				try
				{
					return this.progress(current, total);
				}
				catch (Exception caught)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
					return false;
				}
			}

			internal void ThrowIfFailed()
			{
				if (null != this.exception)
				{
					this.exception.Throw();
				}
			}
		}

		/// <summary>
		/// Runs archive mutations atomically and creates a new snapshot when they succeed.
		/// </summary>
		/// <param name="transaction">Mutations to perform using the reserved snapshot identifier.</param>
		/// <param name="parents">Parent snapshot identifiers for the new snapshot.</param>
		/// <returns>The committed snapshot identifier.</returns>
		public string NewSnapshotTransaction(
			TypeArchiveSnapshotTransaction transaction,
			string[] parents)
		{
			if (null == transaction)
			{
				throw new ArgumentNullException(nameof(transaction));
			}

			if (null == parents)
			{
				throw new ArgumentNullException(nameof(parents));
			}

			for (int i = 0; i < parents.Length; i++)
			{
				if (null == parents[i])
				{
					throw new ArgumentException("Snapshot parents cannot contain null entries.", nameof(parents));
				}
			}

			SnapshotTransactionContext transactionContext =
				new SnapshotTransactionContext(transaction);
			NativeSnapshotTransaction nativeTransaction = transactionContext.Invoke;
			IntPtr result;

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				try
				{
					result = NativeMethods.BNTypeArchiveNewSnapshotTransaction(
						this.handle,
						Marshal.GetFunctionPointerForDelegate<NativeSnapshotTransaction>(nativeTransaction),
						IntPtr.Zero,
						allocator.AllocUtf8StringArray(parents),
						(ulong)parents.Length);
				}
				finally
				{
					GC.KeepAlive(nativeTransaction);
				}
			}

			transactionContext.ThrowIfFailed();
			if (IntPtr.Zero == result)
			{
				throw new InvalidOperationException("BNTypeArchiveNewSnapshotTransaction failed.");
			}

			return UnsafeUtils.TakeUtf8String(result);
		}

		/// <summary>
		/// Merges two snapshots that share a common base snapshot.
		/// </summary>
		/// <param name="baseSnapshot">Common ancestor snapshot identifier.</param>
		/// <param name="firstSnapshot">First snapshot to merge.</param>
		/// <param name="secondSnapshot">Second snapshot to merge.</param>
		/// <param name="mergeConflictResolutions">Type ID to selected snapshot ID resolutions.</param>
		/// <param name="mergeConflicts">Receives unresolved conflicting type IDs.</param>
		/// <param name="progress">Optional merge progress callback.</param>
		/// <returns>The merged snapshot identifier, or null when conflicts remain unresolved.</returns>
		public string? MergeSnapshots(
			string baseSnapshot,
			string firstSnapshot,
			string secondSnapshot,
			IReadOnlyDictionary<string, string>? mergeConflictResolutions,
			out string[] mergeConflicts,
			ProgressDelegate? progress = null)
		{
			if (null == baseSnapshot)
			{
				throw new ArgumentNullException(nameof(baseSnapshot));
			}

			if (null == firstSnapshot)
			{
				throw new ArgumentNullException(nameof(firstSnapshot));
			}

			if (null == secondSnapshot)
			{
				throw new ArgumentNullException(nameof(secondSnapshot));
			}

			int resolutionCount = null == mergeConflictResolutions
				? 0
				: mergeConflictResolutions.Count;
			string[] resolutionKeys = new string[resolutionCount];
			string[] resolutionValues = new string[resolutionCount];
			if (null != mergeConflictResolutions)
			{
				int index = 0;
				foreach (KeyValuePair<string, string> resolution in mergeConflictResolutions)
				{
					if (null == resolution.Key || null == resolution.Value)
					{
						throw new ArgumentException(
							"Merge conflict resolutions cannot contain null keys or values.",
							nameof(mergeConflictResolutions));
					}

					resolutionKeys[index] = resolution.Key;
					resolutionValues[index] = resolution.Value;
					index++;
				}
			}

			IntPtr nativeConflicts = IntPtr.Zero;
			ulong nativeConflictCount = 0;
			IntPtr nativeResult = IntPtr.Zero;
			MergeProgressContext progressContext = new MergeProgressContext(progress);
			NativeDelegates.BNProgressFunction nativeProgress = progressContext.Invoke;
			bool success;

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				try
				{
					success = NativeMethods.BNTypeArchiveMergeSnapshots(
						this.handle,
						baseSnapshot,
						firstSnapshot,
						secondSnapshot,
						allocator.AllocUtf8StringArray(resolutionKeys),
						allocator.AllocUtf8StringArray(resolutionValues),
						(ulong)resolutionCount,
						out nativeConflicts,
						out nativeConflictCount,
						out nativeResult,
						Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress),
						IntPtr.Zero);
				}
				finally
				{
					GC.KeepAlive(nativeProgress);
				}
			}

			string? result;
			try
			{
				mergeConflicts = UnsafeUtils.ReadUtf8StringArray(
					nativeConflicts,
					nativeConflictCount);
				result = IntPtr.Zero == nativeResult
					? null
					: UnsafeUtils.ReadUtf8String(nativeResult);
			}
			finally
			{
				if (IntPtr.Zero != nativeConflicts)
				{
					NativeMethods.BNFreeStringList(nativeConflicts, nativeConflictCount);
				}

				if (IntPtr.Zero != nativeResult)
				{
					NativeMethods.BNFreeString(nativeResult);
				}
			}

			progressContext.ThrowIfFailed();
			if (!success)
			{
				throw new InvalidOperationException("BNTypeArchiveMergeSnapshots failed.");
			}

			return result;
		}
	}
}
