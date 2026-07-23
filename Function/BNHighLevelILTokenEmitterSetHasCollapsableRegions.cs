using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNHighLevelILTokenEmitterSetHasCollapsableRegions(BNHighLevelILTokenEmitter* emitter, bool state)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHighLevelILTokenEmitterSetHasCollapsableRegions"
        )]
		internal static extern void BNHighLevelILTokenEmitterSetHasCollapsableRegions(
			
			// BNHighLevelILTokenEmitter* emitter
		    IntPtr emitter  , 
			
			// bool state
		    [MarshalAs(UnmanagedType.I1)] bool state
		);
	}
}
