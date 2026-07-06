using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetLowLevelILExprText(BNLowLevelILFunction* func, BNArchitecture* arch, uint64_t i, BNDisassemblySettings* settings, BNInstructionTextToken** tokens, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetLowLevelILExprText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetLowLevelILExprText(
			
			// BNLowLevelILFunction* func
		    IntPtr func  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t i
			LowLevelILExpressionIndex i  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNInstructionTextToken** tokens
		    out IntPtr tokens  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}