using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Contains the symbol and optional type produced by a symbol queue resolver.
	/// </summary>
	public sealed class SymbolQueueResolution
	{
		public SymbolQueueResolution(
			Symbol? symbol,
			BinaryNinja.Type? type = null,
			byte typeConfidence = 0)
		{
			this.Symbol = symbol;
			this.Type = type;
			this.TypeConfidence = typeConfidence;
		}

		public Symbol? Symbol { get; }

		public BinaryNinja.Type? Type { get; }

		public byte TypeConfidence { get; }
	}

	/// <summary>
	/// Resolves one deferred symbol queue entry.
	/// </summary>
	public delegate SymbolQueueResolution SymbolQueueResolveDelegate();

	/// <summary>
	/// Applies a resolved symbol queue entry.
	/// </summary>
	public delegate void SymbolQueueAddDelegate(
		Symbol? symbol,
		BinaryNinja.Type? type,
		byte typeConfidence);

	public sealed partial class SymbolQueue
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void NativeResolveCallback(
			IntPtr context,
			IntPtr symbol,
			IntPtr type);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void NativeAddCallback(
			IntPtr context,
			IntPtr symbol,
			IntPtr type);

		private static readonly NativeResolveCallback NativeResolve = ResolveCallback;
		private static readonly NativeAddCallback NativeAdd = AddCallback;

		private readonly object callbackSync = new object();
		private readonly HashSet<IntPtr> pendingEntries = new HashSet<IntPtr>();
		private readonly List<ExceptionDispatchInfo> callbackExceptions =
			new List<ExceptionDispatchInfo>();

		private sealed class QueueEntry
		{
			private readonly SymbolQueue owner;
			private readonly SymbolQueueResolveDelegate resolve;
			private readonly SymbolQueueAddDelegate add;
			private ExceptionDispatchInfo? exception;
			private IntPtr context;

			internal QueueEntry(
				SymbolQueue owner,
				SymbolQueueResolveDelegate resolve,
				SymbolQueueAddDelegate add)
			{
				this.owner = owner;
				this.resolve = resolve;
				this.add = add;
			}

			internal void SetContext(IntPtr context)
			{
				this.context = context;
			}

			internal void Resolve(IntPtr symbolPointer, IntPtr typePointer)
			{
				IntPtr nativeSymbol = IntPtr.Zero;
				BNTypeWithConfidence nativeType = new BNTypeWithConfidence
				{
					type = IntPtr.Zero,
					confidence = 0
				};

				try
				{
					SymbolQueueResolution resolution = this.resolve();
					if (null != resolution)
					{
						if (null != resolution.Symbol)
						{
							nativeSymbol = NativeMethods.BNNewSymbolReference(
								resolution.Symbol.DangerousGetHandle());
						}

						if (null != resolution.Type)
						{
							nativeType.type = NativeMethods.BNNewTypeReference(
								resolution.Type.DangerousGetHandle());
						}

						nativeType.confidence = resolution.TypeConfidence;
					}
				}
				catch (Exception caught)
				{
					this.Capture(caught);
				}

				Marshal.WriteIntPtr(symbolPointer, nativeSymbol);
				Marshal.StructureToPtr<BNTypeWithConfidence>(nativeType, typePointer, false);
			}

			internal void Add(IntPtr symbolPointer, IntPtr typePointer)
			{
				BNTypeWithConfidence nativeType =
					Marshal.PtrToStructure<BNTypeWithConfidence>(typePointer);
				using (Symbol? symbol = Symbol.TakeHandle(symbolPointer))
				using (BinaryNinja.Type? type = BinaryNinja.Type.TakeHandle(nativeType.type))
				{
					if (null == this.exception)
					{
						try
						{
							this.add(symbol, type, nativeType.confidence);
						}
						catch (Exception caught)
						{
							this.Capture(caught);
						}
					}
				}

				this.owner.CompleteEntry(this.context, this.exception);
			}

			private void Capture(Exception caught)
			{
				if (null == this.exception)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
				}
			}
		}

		/// <summary>
		/// Appends a deferred symbol resolution and application pair to the queue.
		/// </summary>
		public void Append(
			SymbolQueueResolveDelegate resolve,
			SymbolQueueAddDelegate add)
		{
			if (null == resolve)
			{
				throw new ArgumentNullException(nameof(resolve));
			}

			if (null == add)
			{
				throw new ArgumentNullException(nameof(add));
			}

			QueueEntry entry = new QueueEntry(this, resolve, add);
			GCHandle entryHandle = GCHandle.Alloc(entry);
			IntPtr context = GCHandle.ToIntPtr(entryHandle);
			entry.SetContext(context);
			lock (this.callbackSync)
			{
				this.pendingEntries.Add(context);
			}

			try
			{
				NativeMethods.BNAppendSymbolQueue(
					this.handle,
					Marshal.GetFunctionPointerForDelegate<NativeResolveCallback>(NativeResolve),
					context,
					Marshal.GetFunctionPointerForDelegate<NativeAddCallback>(NativeAdd),
					context);
			}
			catch (Exception)
			{
				lock (this.callbackSync)
				{
					this.pendingEntries.Remove(context);
				}

				entryHandle.Free();
				throw;
			}
		}

		/// <summary>
		/// Processes every queued entry and rethrows the first managed callback exception.
		/// </summary>
		public void Process()
		{
			NativeMethods.BNProcessSymbolQueue(this.handle);
			ExceptionDispatchInfo? exception = null;
			lock (this.callbackSync)
			{
				if (0 < this.callbackExceptions.Count)
				{
					exception = this.callbackExceptions[0];
					this.callbackExceptions.Clear();
				}
			}

			if (null != exception)
			{
				exception.Throw();
			}
		}

		private static void ResolveCallback(
			IntPtr context,
			IntPtr symbol,
			IntPtr type)
		{
			QueueEntry? entry = GetEntry(context);
			if (null != entry)
			{
				entry.Resolve(symbol, type);
			}
		}

		private static void AddCallback(
			IntPtr context,
			IntPtr symbol,
			IntPtr type)
		{
			QueueEntry? entry = GetEntry(context);
			if (null != entry)
			{
				entry.Add(symbol, type);
			}
		}

		private static QueueEntry? GetEntry(IntPtr context)
		{
			if (IntPtr.Zero == context)
			{
				return null;
			}

			GCHandle entryHandle = GCHandle.FromIntPtr(context);
			return entryHandle.Target as QueueEntry;
		}

		private void CompleteEntry(
			IntPtr context,
			ExceptionDispatchInfo? exception)
		{
			bool removed;
			lock (this.callbackSync)
			{
				removed = this.pendingEntries.Remove(context);
				if (null != exception)
				{
					this.callbackExceptions.Add(exception);
				}
			}

			if (removed)
			{
				GCHandle entryHandle = GCHandle.FromIntPtr(context);
				entryHandle.Free();
			}
		}

		private void DisposePendingEntries()
		{
			IntPtr[] entries;
			lock (this.callbackSync)
			{
				entries = new IntPtr[this.pendingEntries.Count];
				this.pendingEntries.CopyTo(entries);
				this.pendingEntries.Clear();
				this.callbackExceptions.Clear();
			}

			for (int i = 0; i < entries.Length; i++)
			{
				GCHandle entryHandle = GCHandle.FromIntPtr(entries[i]);
				entryHandle.Free();
			}
		}
	}
}
