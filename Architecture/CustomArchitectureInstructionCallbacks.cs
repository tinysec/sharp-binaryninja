using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool GetInstructionInfoCallback(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong maximumLength,
			IntPtr result);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool GetInstructionTextCallback(
			IntPtr context,
			IntPtr data,
			ulong address,
			ref ulong length,
			out IntPtr tokens,
			out ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void FreeInstructionTextCallback(IntPtr tokens, ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool GetInstructionLowLevelILCallback(
			IntPtr context,
			IntPtr data,
			ulong address,
			ref ulong length,
			IntPtr il);

		private void AddInstructionCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.getInstructionInfo = UnsafeUtils.PinCallback<GetInstructionInfoCallback>(
				this.GetInstructionInfoAdapter);
			callbacks.getInstructionText = UnsafeUtils.PinCallback<GetInstructionTextCallback>(
				this.GetInstructionTextAdapter);
			callbacks.freeInstructionText = UnsafeUtils.PinCallback<FreeInstructionTextCallback>(
				this.FreeInstructionTextAdapter);
			callbacks.getInstructionLowLevelIL =
				UnsafeUtils.PinCallback<GetInstructionLowLevelILCallback>(
					this.GetInstructionLowLevelILAdapter);
		}

		private bool GetInstructionTextAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ref ulong length,
			out IntPtr tokens,
			out ulong count)
		{
			tokens = IntPtr.Zero;
			count = 0;

			try
			{
				ulong maximumLength = length;
				byte[] bytes = new byte[checked((int)maximumLength)];
				if (0 != bytes.Length)
				{
					Marshal.Copy(data, bytes, 0, bytes.Length);
				}

				InstructionTextToken[] managedTokens =
					this.GetInstructionText(bytes, address, out ulong decodedLength)
					?? Array.Empty<InstructionTextToken>();
				if (0 == decodedLength || maximumLength < decodedLength)
				{
					return false;
				}

				tokens = this.AllocateInstructionText(managedTokens);
				count = (ulong)managedTokens.Length;
				length = decodedLength;

				return true;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetInstructionText: {0}",
					exception);
				tokens = IntPtr.Zero;
				count = 0;

				return false;
			}
		}

		private bool GetInstructionLowLevelILAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ref ulong length,
			IntPtr ilHandle)
		{
			try
			{
				ulong maximumLength = length;
				byte[] bytes = new byte[checked((int)maximumLength)];
				if (0 != bytes.Length)
				{
					Marshal.Copy(data, bytes, 0, bytes.Length);
				}

				using (LowLevelILFunction il = this.CreateCallbackLowLevelIL(ilHandle))
				{
					ulong? decodedLength = this.GetInstructionLowLevelIL(bytes, address, il);
					if (null == decodedLength
						|| 0 == decodedLength.Value
						|| maximumLength < decodedLength.Value)
					{
						return false;
					}

					length = decodedLength.Value;

					return true;
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetInstructionLowLevelIL: {0}",
					exception);

				return false;
			}
		}

		private IntPtr AllocateInstructionText(InstructionTextToken[] tokens)
		{
			BNInstructionTextToken[] nativeTokens = new BNInstructionTextToken[tokens.Length];
			List<IntPtr> allocatedStrings = new List<IntPtr>();
			List<IntPtr> allocatedArrays = new List<IntPtr>();

			try
			{
				for (int index = 0; index < tokens.Length; index++)
				{
					InstructionTextToken token = tokens[index];
					IntPtr text = NativeMethods.BNAllocString(token.Text);
					allocatedStrings.Add(text);

					IntPtr typeNames = this.AllocateTypeNames(
						token.TypeNames,
						allocatedStrings);
					if (IntPtr.Zero != typeNames)
					{
						allocatedArrays.Add(typeNames);
					}

					nativeTokens[index] = new BNInstructionTextToken
					{
						type = token.Type,
						text = text,
						value = token.Value,
						width = token.Width,
						size = token.Size,
						operand = token.Operand,
						context = token.Context,
						confidence = token.Confidence,
						address = token.Address,
						typeNames = typeNames,
						namesCount = (ulong)token.TypeNames.Length,
						exprIndex = token.ExpressionIndex
					};
				}

				return UnsafeUtils.AllocStructArray(nativeTokens);
			}
			catch
			{
				this.FreeAllocatedInstructionText(allocatedStrings, allocatedArrays);
				throw;
			}
		}

		private IntPtr AllocateTypeNames(
			string[] typeNames,
			List<IntPtr> allocatedStrings)
		{
			if (0 == typeNames.Length)
			{
				return IntPtr.Zero;
			}

			IntPtr result = Marshal.AllocHGlobal(checked(typeNames.Length * IntPtr.Size));
			try
			{
				for (int index = 0; index < typeNames.Length; index++)
				{
					IntPtr name = NativeMethods.BNAllocString(typeNames[index]);
					allocatedStrings.Add(name);
					Marshal.WriteIntPtr(result, checked(index * IntPtr.Size), name);
				}

				return result;
			}
			catch
			{
				Marshal.FreeHGlobal(result);
				throw;
			}
		}

		private void FreeAllocatedInstructionText(
			List<IntPtr> strings,
			List<IntPtr> arrays)
		{
			foreach (IntPtr text in strings)
			{
				if (IntPtr.Zero != text)
				{
					NativeMethods.BNFreeString(text);
				}
			}

			foreach (IntPtr array in arrays)
			{
				Marshal.FreeHGlobal(array);
			}
		}

		private void FreeInstructionTextAdapter(IntPtr tokens, ulong count)
		{
			if (IntPtr.Zero == tokens)
			{
				return;
			}

			int tokenSize = Marshal.SizeOf<BNInstructionTextToken>();
			for (ulong index = 0; index < count; index++)
			{
				IntPtr tokenPointer = IntPtr.Add(
					tokens,
					checked((int)(index * (ulong)tokenSize)));
				BNInstructionTextToken token =
					Marshal.PtrToStructure<BNInstructionTextToken>(tokenPointer);

				if (IntPtr.Zero != token.text)
				{
					NativeMethods.BNFreeString(token.text);
				}

				for (ulong nameIndex = 0; nameIndex < token.namesCount; nameIndex++)
				{
					IntPtr name = Marshal.ReadIntPtr(
						token.typeNames,
						checked((int)(nameIndex * (ulong)IntPtr.Size)));
					if (IntPtr.Zero != name)
					{
						NativeMethods.BNFreeString(name);
					}
				}

				if (IntPtr.Zero != token.typeNames)
				{
					Marshal.FreeHGlobal(token.typeNames);
				}
			}

			Marshal.FreeHGlobal(tokens);
		}

		private bool GetInstructionInfoAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong maximumLength,
			IntPtr result)
		{
			try
			{
				byte[] bytes = new byte[checked((int)maximumLength)];
				if (0 != bytes.Length)
				{
					Marshal.Copy(data, bytes, 0, bytes.Length);
				}

				InstructionInfo? info = this.GetInstructionInfo(bytes, address);
				if (null == info)
				{
					return false;
				}

				Marshal.StructureToPtr(info.ToNative(), result, false);

				return true;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetInstructionInfo: {0}",
					exception);

				return false;
			}
		}
	}
}
