using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetMediumLevelILInstructionText(BNMediumLevelILFunction* il, BNFunction* func, BNArchitecture* arch, uint64_t i, BNInstructionTextToken** tokens, uint64_t* count, BNDisassemblySettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetMediumLevelILInstructionText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetMediumLevelILInstructionText(
			
			// BNMediumLevelILFunction* il
		    IntPtr il  , 
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t i
		    ulong i  , 
			
			// BNInstructionTextToken** tokens
		    IntPtr tokens  , 
			
			// uint64_t* count
		    IntPtr count  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  
			
		);
	}
}