using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNHighLevelILTokenEmitterScopeContinuation(BNHighLevelILTokenEmitter* emitter, bool forceSameLine)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHighLevelILTokenEmitterScopeContinuation"
        )]
		internal static extern void BNHighLevelILTokenEmitterScopeContinuation(
			
			// BNHighLevelILTokenEmitter* emitter
		    IntPtr emitter  , 
			
			// bool forceSameLine
		    [MarshalAs(UnmanagedType.I1)] bool forceSameLine
		);
	}
}
