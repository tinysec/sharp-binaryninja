using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetStackVariableAtFrameOffsetAfterInstruction(BNFunction* func, BNArchitecture* arch, uint64_t addr, int64_t offset, BNVariableNameAndType* var)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetStackVariableAtFrameOffsetAfterInstruction"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetStackVariableAtFrameOffsetAfterInstruction(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// int64_t offset
		    long offset  , 
			
			// BNVariableNameAndType* _var
		    IntPtr _var  
			
		);
	}
}