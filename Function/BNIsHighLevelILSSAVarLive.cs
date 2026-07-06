using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsHighLevelILSSAVarLive(BNHighLevelILFunction* func, BNVariable* var, uint64_t version)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsHighLevelILSSAVarLive"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsHighLevelILSSAVarLive(
			
			// BNHighLevelILFunction* func
		    IntPtr func  , 
			
			// BNVariable* _var
		    in BNVariable _var  , 
			
			// uint64_t version
		    ulong version  
		);
	}
}