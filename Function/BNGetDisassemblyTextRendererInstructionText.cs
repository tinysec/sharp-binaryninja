using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetDisassemblyTextRendererInstructionText(BNDisassemblyTextRenderer* renderer, uint64_t addr, uint64_t* len, BNDisassemblyTextLine** result, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDisassemblyTextRendererInstructionText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetDisassemblyTextRendererInstructionText(
			
			// BNDisassemblyTextRenderer* renderer
		    IntPtr renderer  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t* len
		    IntPtr len  , 
			
			// BNDisassemblyTextLine** result
		    IntPtr result  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}