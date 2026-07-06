using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetLowLevelILInstructionText(BNLowLevelILFunction* il, BNFunction* func, BNArchitecture* arch, uint64_t i, BNDisassemblySettings* settings, BNInstructionTextToken** tokens, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetLowLevelILInstructionText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetLowLevelILInstructionText(
			
			// BNLowLevelILFunction* il
		    IntPtr il  , 
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t i
		    LowLevelILInstructionIndex i  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNInstructionTextToken** tokens
		    out IntPtr tokens  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}