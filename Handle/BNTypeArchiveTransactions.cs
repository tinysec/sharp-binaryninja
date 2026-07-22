using System;
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
	}
}
