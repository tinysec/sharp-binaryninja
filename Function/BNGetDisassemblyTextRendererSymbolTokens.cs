using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetDisassemblyTextRendererSymbolTokens(BNDisassemblyTextRenderer* renderer, uint64_t addr, uint64_t size, uint64_t operand, BNInstructionTextToken** result, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDisassemblyTextRendererSymbolTokens"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetDisassemblyTextRendererSymbolTokens(
			
			// BNDisassemblyTextRenderer* renderer
		    IntPtr renderer  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t size
		    ulong size  , 
			
			// uint64_t operand
		    ulong operand  , 
			
			// BNInstructionTextToken** result
		    IntPtr result  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}