using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDisassemblyTextLine* BNPostProcessDisassemblyTextRendererLines(BNDisassemblyTextRenderer* renderer, uint64_t addr, uint64_t len, BNDisassemblyTextLine* inLines, uint64_t inCount, uint64_t* outCount, const char* indentSpaces)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPostProcessDisassemblyTextRendererLines"
        )]
		internal static extern IntPtr BNPostProcessDisassemblyTextRendererLines(
			
			// BNDisassemblyTextRenderer* renderer
		    IntPtr renderer  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t len
		    ulong len  , 
			
			// BNDisassemblyTextLine* inLines
		    IntPtr inLines  , 
			
			// uint64_t inCount
		    ulong inCount  , 
			
			// uint64_t* outCount
		    IntPtr outCount  , 
			
			// const char* indentSpaces
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string indentSpaces  
			
		);
	}
}