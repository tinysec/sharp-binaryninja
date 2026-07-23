using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNHighLevelILTokenEmitterSetBracesAroundSwitchCases(BNHighLevelILTokenEmitter* emitter, bool braces)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNHighLevelILTokenEmitterSetBracesAroundSwitchCases"
        )]
		internal static extern void BNHighLevelILTokenEmitterSetBracesAroundSwitchCases(
			
			// BNHighLevelILTokenEmitter* emitter
		    IntPtr emitter  , 
			
			// bool braces
		    [MarshalAs(UnmanagedType.I1)] bool braces
			
		);
	}
}
