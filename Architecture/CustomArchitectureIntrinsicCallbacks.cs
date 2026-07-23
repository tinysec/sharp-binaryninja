using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntrinsicClass GetIntrinsicClassCallback(
			IntPtr context,
			uint intrinsic);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr GetIntrinsicTypesCallback(
			IntPtr context,
			uint intrinsic,
			out ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void FreeIntrinsicTypesCallback(
			IntPtr context,
			IntPtr types,
			ulong count);

		private void AddIntrinsicCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.getIntrinsicClass = UnsafeUtils.PinCallback<GetIntrinsicClassCallback>(
				this.GetIntrinsicClassAdapter);
			callbacks.getIntrinsicName = UnsafeUtils.PinCallback<GetRegisterNameCallback>(
				this.GetIntrinsicNameAdapter);
			callbacks.getAllIntrinsics = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetAllIntrinsicsAdapter);
			callbacks.getIntrinsicInputs = UnsafeUtils.PinCallback<GetIntrinsicTypesCallback>(
				this.GetIntrinsicInputsAdapter);
			callbacks.freeNameAndTypeList = UnsafeUtils.PinCallback<FreeIntrinsicTypesCallback>(
				this.FreeIntrinsicInputsAdapter);
			callbacks.getIntrinsicOutputs = UnsafeUtils.PinCallback<GetIntrinsicTypesCallback>(
				this.GetIntrinsicOutputsAdapter);
			callbacks.freeTypeList = UnsafeUtils.PinCallback<FreeIntrinsicTypesCallback>(
				this.FreeIntrinsicOutputsAdapter);
		}

		private IntrinsicClass GetIntrinsicClassAdapter(IntPtr context, uint intrinsic)
		{
			try
			{
				return this.GetIntrinsicClass(intrinsic);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetIntrinsicClass: {0}",
					exception);

				return IntrinsicClass.GeneralIntrinsicClass;
			}
		}

		private IntPtr GetIntrinsicNameAdapter(IntPtr context, uint intrinsic)
		{
			try
			{
				return NativeMethods.BNAllocString(this.GetIntrinsicName(intrinsic));
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetIntrinsicName: {0}",
					exception);

				return NativeMethods.BNAllocString(string.Empty);
			}
		}

		private IntPtr GetAllIntrinsicsAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetAllIntrinsics,
				"GetAllIntrinsics",
				out count);
		}

		private IntPtr GetIntrinsicInputsAdapter(
			IntPtr context,
			uint intrinsic,
			out ulong count)
		{
			BNNameAndType[] nativeInputs = Array.Empty<BNNameAndType>();
			try
			{
				NameAndType[] inputs = this.GetIntrinsicInputs(intrinsic)
					?? Array.Empty<NameAndType>();
				nativeInputs = new BNNameAndType[inputs.Length];
				for (int index = 0; index < inputs.Length; index++)
				{
					nativeInputs[index].name = NativeMethods.BNAllocString(inputs[index].Name);
					nativeInputs[index].type = NativeMethods.BNNewTypeReference(
						inputs[index].Type.DangerousGetHandle());
					nativeInputs[index].typeConfidence = inputs[index].TypeConfidence;
				}

				count = (ulong)nativeInputs.Length;

				return UnsafeUtils.AllocStructArray(nativeInputs);
			}
			catch (Exception exception)
			{
				this.FreeIntrinsicInputValues(nativeInputs);
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetIntrinsicInputs: {0}",
					exception);
				count = 0;

				return IntPtr.Zero;
			}
		}

		private void FreeIntrinsicInputsAdapter(
			IntPtr context,
			IntPtr inputs,
			ulong count)
		{
			if (IntPtr.Zero == inputs)
			{
				return;
			}

			BNNameAndType[] nativeInputs = UnsafeUtils.ReadStructArray<
				BNNameAndType,
				BNNameAndType>(
				inputs,
				count,
				CustomArchitecture.KeepNameAndType);
			this.FreeIntrinsicInputValues(nativeInputs);
			Marshal.FreeHGlobal(inputs);
		}

		private void FreeIntrinsicInputValues(BNNameAndType[] inputs)
		{
			foreach (BNNameAndType input in inputs)
			{
				if (IntPtr.Zero != input.name)
				{
					NativeMethods.BNFreeString(input.name);
				}

				if (IntPtr.Zero != input.type)
				{
					NativeMethods.BNFreeType(input.type);
				}
			}
		}

		private static BNNameAndType KeepNameAndType(BNNameAndType input)
		{
			return input;
		}

		private IntPtr GetIntrinsicOutputsAdapter(
			IntPtr context,
			uint intrinsic,
			out ulong count)
		{
			BNTypeWithConfidence[] nativeOutputs = Array.Empty<BNTypeWithConfidence>();
			try
			{
				TypeWithConfidence[] outputs = this.GetIntrinsicOutputs(intrinsic)
					?? Array.Empty<TypeWithConfidence>();
				nativeOutputs = new BNTypeWithConfidence[outputs.Length];
				for (int index = 0; index < outputs.Length; index++)
				{
					nativeOutputs[index].type = NativeMethods.BNNewTypeReference(
						outputs[index].Type.DangerousGetHandle());
					nativeOutputs[index].confidence = outputs[index].Confidence;
				}

				count = (ulong)nativeOutputs.Length;

				return UnsafeUtils.AllocStructArray(nativeOutputs);
			}
			catch (Exception exception)
			{
				this.FreeIntrinsicOutputValues(nativeOutputs);
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetIntrinsicOutputs: {0}",
					exception);
				count = 0;

				return IntPtr.Zero;
			}
		}

		private void FreeIntrinsicOutputsAdapter(
			IntPtr context,
			IntPtr outputs,
			ulong count)
		{
			if (IntPtr.Zero == outputs)
			{
				return;
			}

			BNTypeWithConfidence[] nativeOutputs = UnsafeUtils.ReadStructArray<
				BNTypeWithConfidence,
				BNTypeWithConfidence>(
				outputs,
				count,
				CustomArchitecture.KeepTypeWithConfidence);
			this.FreeIntrinsicOutputValues(nativeOutputs);
			Marshal.FreeHGlobal(outputs);
		}

		private void FreeIntrinsicOutputValues(BNTypeWithConfidence[] outputs)
		{
			foreach (BNTypeWithConfidence output in outputs)
			{
				if (IntPtr.Zero != output.type)
				{
					NativeMethods.BNFreeType(output.type);
				}
			}
		}

		private static BNTypeWithConfidence KeepTypeWithConfidence(
			BNTypeWithConfidence output)
		{
			return output;
		}
	}
}
