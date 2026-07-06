using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsHighLevelILVarLiveAt(BNHighLevelILFunction* func, BNVariable* var, uint64_t instr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsHighLevelILVarLiveAt"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsHighLevelILVarLiveAt(
			
			// BNHighLevelILFunction* func
		    IntPtr func  , 
			
			// BNVariable* _var
		    in BNVariable _var  , 
			
			// uint64_t instr
			HighLevelILInstructionIndex instr  
		);
	}
}