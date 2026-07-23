using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNHighLevelILTokenEmitterSetSimpleScopeAllowed(BNHighLevelILTokenEmitter* emitter, bool allowed)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNHighLevelILTokenEmitterSetSimpleScopeAllowed"
        )]
		internal static extern void BNHighLevelILTokenEmitterSetSimpleScopeAllowed(
			
			// BNHighLevelILTokenEmitter* emitter
		    IntPtr emitter  , 
			
			// bool allowed
		    [MarshalAs(UnmanagedType.I1)] bool allowed
			
		);
	}
}
