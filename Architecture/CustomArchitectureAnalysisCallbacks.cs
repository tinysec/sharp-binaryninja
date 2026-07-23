using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void AnalyzeBasicBlocksCallback(
			IntPtr context,
			IntPtr function,
			IntPtr analysisContext);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool LiftFunctionCallback(
			IntPtr context,
			IntPtr function,
			IntPtr lifterContext);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void FreeFunctionArchitectureContextCallback(
			IntPtr context,
			IntPtr functionContext);

		private void AddAnalysisCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.analyzeBasicBlocks = UnsafeUtils.PinCallback<AnalyzeBasicBlocksCallback>(
				this.AnalyzeBasicBlocksAdapter);
			callbacks.liftFunction = UnsafeUtils.PinCallback<LiftFunctionCallback>(
				this.LiftFunctionAdapter);
			callbacks.freeFunctionArchContext =
				UnsafeUtils.PinCallback<FreeFunctionArchitectureContextCallback>(
					this.FreeFunctionArchitectureContextAdapter);
		}

		private void AnalyzeBasicBlocksAdapter(
			IntPtr context,
			IntPtr functionHandle,
			IntPtr analysisContextHandle)
		{
			try
			{
				using (Function function = Function.MustNewFromHandle(functionHandle))
				{
					BasicBlockAnalysisContext analysisContext =
						new BasicBlockAnalysisContext(analysisContextHandle);
					this.AnalyzeBasicBlocks(function, analysisContext);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.AnalyzeBasicBlocks: {0}",
					exception);
			}
		}

		private bool LiftFunctionAdapter(
			IntPtr context,
			IntPtr functionHandle,
			IntPtr lifterContextHandle)
		{
			try
			{
				using (LowLevelILFunction function =
					this.CreateCallbackLowLevelIL(functionHandle))
				{
					FunctionLifterContext lifterContext = new FunctionLifterContext(
						lifterContextHandle,
						function);
					return this.LiftFunction(function, lifterContext);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.LiftFunction: {0}",
					exception);
				return false;
			}
		}

		private void FreeFunctionArchitectureContextAdapter(
			IntPtr context,
			IntPtr functionContext)
		{
			try
			{
					this.FreeFunctionArchContext(functionContext);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.FreeFunctionArchContext: {0}",
					exception);
			}
		}
	}
}
