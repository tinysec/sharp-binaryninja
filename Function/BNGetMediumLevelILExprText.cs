using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetMediumLevelILExprText(BNMediumLevelILFunction* func, BNArchitecture* arch, uint64_t i, BNInstructionTextToken** tokens, uint64_t* count, BNDisassemblySettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetMediumLevelILExprText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetMediumLevelILExprText(
			
			// BNMediumLevelILFunction* func
		    IntPtr func  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t i
			MediumLevelILExpressionIndex i  , 
			
			// BNInstructionTextToken** tokens
		    out IntPtr tokens  , 
			
			// uint64_t* count
		    out ulong count  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  
		);
	}
}