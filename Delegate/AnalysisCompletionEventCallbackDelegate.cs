using System;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		// void (*callback)(void* ctxt)
		// Invoked once by the core when an analysis pass completes. Mirrors the native callback
		// registered by Python AnalysisCompletionEvent (binaryview.py:688).
		public delegate void AnalysisCompletionEventCallbackDelegate(IntPtr context);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a public Action into the native analysis-completion callback shape. The core
		// passes only the opaque context pointer, which the binding does not need (the user Action
		// captures whatever state it requires), so it is ignored.
		internal sealed class AnalysisCompletionEventContext
		{
			private readonly Action m_callback;

			internal AnalysisCompletionEventContext(Action callback)
			{
				this.m_callback = callback;
			}

			internal void OnNotify(IntPtr context)
			{
				this.m_callback();
			}
		}

		internal static NativeDelegates.AnalysisCompletionEventCallbackDelegate WrapAnalysisCompletionEventCallback(
			Action callback)
		{
			AnalysisCompletionEventContext context = new AnalysisCompletionEventContext(callback);

			return new NativeDelegates.AnalysisCompletionEventCallbackDelegate(context.OnNotify);
		}
	}
}
