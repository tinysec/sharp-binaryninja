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

		private void AddAnalysisCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.analyzeBasicBlocks = UnsafeUtils.PinCallback<AnalyzeBasicBlocksCallback>(
				this.AnalyzeBasicBlocksAdapter);
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
	}
}
