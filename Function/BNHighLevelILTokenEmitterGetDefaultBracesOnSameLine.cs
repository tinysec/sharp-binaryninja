using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNHighLevelILTokenEmitterGetDefaultBracesOnSameLine(BNHighLevelILTokenEmitter* emitter)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNHighLevelILTokenEmitterGetDefaultBracesOnSameLine"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNHighLevelILTokenEmitterGetDefaultBracesOnSameLine(
			
			// BNHighLevelILTokenEmitter* emitter
		    IntPtr emitter  
			
		);
	}
}