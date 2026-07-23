using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNHighLevelILTokenEmitterSetDefaultBracesOnSameLine(BNHighLevelILTokenEmitter* emitter, bool sameLine)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNHighLevelILTokenEmitterSetDefaultBracesOnSameLine"
        )]
		internal static extern void BNHighLevelILTokenEmitterSetDefaultBracesOnSameLine(
			
			// BNHighLevelILTokenEmitter* emitter
		    IntPtr emitter  , 
			
			// bool sameLine
		    [MarshalAs(UnmanagedType.I1)] bool sameLine
			
		);
	}
}
