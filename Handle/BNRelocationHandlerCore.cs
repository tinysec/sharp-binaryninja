using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public partial class RelocationHandler
	{
		private sealed class CoreRelocationHandler : RelocationHandler
		{
			internal CoreRelocationHandler(IntPtr handle, bool owner)
				: base(handle, owner)
			{
			}

			public override bool GetRelocationInfo(
				BinaryView view,
				Architecture architecture,
				RelocationInfo[] result
			)
			{
				return this.GetRelocationInfoCore(view, architecture, result);
			}

			public override bool ApplyRelocation(
				BinaryView view,
				Architecture architecture,
				Relocation relocation,
				IntPtr destination,
				ulong length
			)
			{
				RelocationHandler.ValidateRelocationArguments(
					view, architecture, relocation
				);
				return NativeMethods.BNRelocationHandlerApplyRelocation(
					this.handle,
					view.DangerousGetHandle(),
					architecture.DangerousGetHandle(),
					relocation.DangerousGetHandle(),
					destination,
					length
				);
			}

			public override ulong GetOperandForExternalRelocation(
				IntPtr data,
				ulong address,
				ulong length,
				LowLevelILFunction lowLevelIl,
				Relocation relocation
			)
			{
				if (null == lowLevelIl)
				{
					throw new ArgumentNullException(nameof(lowLevelIl));
				}

				if (null == relocation)
				{
					throw new ArgumentNullException(nameof(relocation));
				}

				return NativeMethods.BNRelocationHandlerGetOperandForExternalRelocation(
					this.handle,
					data,
					address,
					length,
					lowLevelIl.DangerousGetHandle(),
					relocation.DangerousGetHandle()
				);
			}
		}

		private bool GetRelocationInfoCore(
			BinaryView view,
			Architecture architecture,
			RelocationInfo[] result
		)
		{
			RelocationHandler.ValidateRelocationInfoArguments(
				view, architecture, result
			);
			BNRelocationInfo[] nativeResult =
				RelocationHandler.CreateNativeRelocationInfo(result);
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr nativePointer = allocator.AllocStructArray(nativeResult);
				bool success = NativeMethods.BNRelocationHandlerGetRelocationInfo(
					this.handle,
					view.DangerousGetHandle(),
					architecture.DangerousGetHandle(),
					nativePointer,
					(ulong)nativeResult.Length
				);
				if (!success)
				{
					return false;
				}

				int nativeSize = Marshal.SizeOf<BNRelocationInfo>();
				for (int i = 0; i < result.Length; i++)
				{
					BNRelocationInfo native = Marshal.PtrToStructure<BNRelocationInfo>(
						IntPtr.Add(nativePointer, i * nativeSize)
					);
					result[i] = RelocationInfo.FromNative(native);
				}

				return true;
			}
		}
	}
}
