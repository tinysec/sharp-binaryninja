using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public partial class RelocationHandler
	{
		private NativeDelegates.BNRelocationHandlerFreeObject? freeObjectCallback;
		private NativeDelegates.BNRelocationHandlerGetRelocationInfo?
			getRelocationInfoCallback;
		private NativeDelegates.BNRelocationHandlerApplyRelocation?
			applyRelocationCallback;
		private NativeDelegates.BNRelocationHandlerGetOperandForExternalRelocation?
			getOperandForExternalRelocationCallback;

		private void InitializeCustomCallbacks()
		{
			this.freeObjectCallback =
				new NativeDelegates.BNRelocationHandlerFreeObject(this.InvokeFreeObject);
			this.getRelocationInfoCallback =
				new NativeDelegates.BNRelocationHandlerGetRelocationInfo(
					this.InvokeGetRelocationInfo
				);
			this.applyRelocationCallback =
				new NativeDelegates.BNRelocationHandlerApplyRelocation(
					this.InvokeApplyRelocation
				);
			this.getOperandForExternalRelocationCallback =
				new NativeDelegates.BNRelocationHandlerGetOperandForExternalRelocation(
					this.InvokeGetOperandForExternalRelocation
				);

			BNCustomRelocationHandler callbacks = new BNCustomRelocationHandler();
			callbacks.context = IntPtr.Zero;
			callbacks.freeObject = Marshal.GetFunctionPointerForDelegate(
				this.freeObjectCallback
			);
			callbacks.getRelocationInfo = Marshal.GetFunctionPointerForDelegate(
				this.getRelocationInfoCallback
			);
			callbacks.applyRelocation = Marshal.GetFunctionPointerForDelegate(
				this.applyRelocationCallback
			);
			callbacks.getOperandForExternalRelocation =
				Marshal.GetFunctionPointerForDelegate(
					this.getOperandForExternalRelocationCallback
				);

			IntPtr customHandle = NativeMethods.BNCreateRelocationHandler(in callbacks);
			if (IntPtr.Zero == customHandle)
			{
				throw new InvalidOperationException(
					"The core rejected the relocation handler."
				);
			}

			this.SetHandle(customHandle);
		}

		private void InvokeFreeObject(IntPtr context)
		{
		}

		private bool InvokeGetRelocationInfo(
			IntPtr context,
			IntPtr view,
			IntPtr architecture,
			IntPtr result,
			ulong resultCount
		)
		{
			if (IntPtr.Zero == result && 0 != resultCount)
			{
				return false;
			}

			BinaryView? managedView = null;
			try
			{
				managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				);
				Architecture managedArchitecture =
					Architecture.MustFromHandle(architecture);
				int count = checked((int)resultCount);
				int nativeSize = Marshal.SizeOf<BNRelocationInfo>();
				BNRelocationInfo[] nativeResult = new BNRelocationInfo[count];
				RelocationInfo[] managedResult = new RelocationInfo[count];
				for (int i = 0; i < count; i++)
				{
					nativeResult[i] = Marshal.PtrToStructure<BNRelocationInfo>(
						IntPtr.Add(result, i * nativeSize)
					);
					managedResult[i] = RelocationInfo.FromNative(nativeResult[i]);
				}

				bool success = this.GetRelocationInfo(
					managedView, managedArchitecture, managedResult
				);
				for (int i = 0; i < count; i++)
				{
					if (null == managedResult[i])
					{
						throw new InvalidOperationException(
							"Relocation metadata cannot contain null values."
						);
					}

					BNRelocationInfo native = managedResult[i].ToNative(nativeResult[i]);
					Marshal.StructureToPtr(
						native, IntPtr.Add(result, i * nativeSize), false
					);
				}

				return success;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in RelocationHandler.GetRelocationInfo: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedView)
				{
					managedView.Dispose();
				}
			}
		}

		private bool InvokeApplyRelocation(
			IntPtr context,
			IntPtr view,
			IntPtr architecture,
			IntPtr relocation,
			IntPtr destination,
			ulong length
		)
		{
			BinaryView? managedView = null;
			Relocation? managedRelocation = null;
			try
			{
				managedView = BinaryView.MustTakeHandle(
					NativeMethods.BNNewViewReference(view)
				);
				Architecture managedArchitecture =
					Architecture.MustFromHandle(architecture);
				managedRelocation = Relocation.MustNewFromHandle(relocation);
				return this.ApplyRelocation(
					managedView,
					managedArchitecture,
					managedRelocation,
					destination,
					length
				);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in RelocationHandler.ApplyRelocation: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedRelocation)
				{
					managedRelocation.Dispose();
				}

				if (null != managedView)
				{
					managedView.Dispose();
				}
			}
		}

		private ulong InvokeGetOperandForExternalRelocation(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length,
			IntPtr lowLevelIl,
			IntPtr relocation
		)
		{
			LowLevelILFunction? managedLowLevelIl = null;
			Relocation? managedRelocation = null;
			try
			{
				managedLowLevelIl = LowLevelILFunction.MustNewFromHandle(lowLevelIl);
				managedRelocation = Relocation.MustNewFromHandle(relocation);
				return this.GetOperandForExternalRelocation(
					data,
					address,
					length,
					managedLowLevelIl,
					managedRelocation
				);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in RelocationHandler.GetOperandForExternalRelocation: {0}",
					exception
				);
				return RelocationHandler.AutoCoerceExternPointer;
			}
			finally
			{
				if (null != managedRelocation)
				{
					managedRelocation.Dispose();
				}

				if (null != managedLowLevelIl)
				{
					managedLowLevelIl.Dispose();
				}
			}
		}
	}
}
