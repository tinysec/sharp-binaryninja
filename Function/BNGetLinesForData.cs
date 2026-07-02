using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDisassemblyTextLine* BNGetLinesForData(void* ctxt, BNBinaryView* view, uint64_t addr, BNType* type, BNInstructionTextToken* prefix, uint64_t prefixCount, uint64_t width, uint64_t* count, BNTypeContext* typeCtx, uint64_t ctxCount, const char* language)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetLinesForData"
        )]
		internal static extern IntPtr BNGetLinesForData(
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// BNInstructionTextToken* prefix
		    IntPtr prefix  , 
			
			// uint64_t prefixCount
		    ulong prefixCount  , 
			
			// uint64_t width
		    ulong width  , 
			
			// uint64_t* count
		    IntPtr count  , 
			
			// BNTypeContext* typeCtx
		    IntPtr typeCtx  , 
			
			// uint64_t ctxCount
		    ulong ctxCount  , 
			
			// const char* language
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string language  
			
		);
	}
}