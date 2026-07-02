using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDisassemblyTextLine* BNDisassemblyTextRendererWrapComment(BNDisassemblyTextRenderer* renderer, BNDisassemblyTextLine* inLine, uint64_t* outLineCount, const char* comment, bool hasAutoAnnotations, const char* leadingSpaces, const char* indentSpaces)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDisassemblyTextRendererWrapComment"
        )]
		internal static extern IntPtr BNDisassemblyTextRendererWrapComment(
			
			// BNDisassemblyTextRenderer* renderer
		    IntPtr renderer  , 
			
			// BNDisassemblyTextLine* inLine
		    IntPtr inLine  , 
			
			// uint64_t* outLineCount
		    IntPtr outLineCount  , 
			
			// const char* comment
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string comment  , 
			
			// bool hasAutoAnnotations
		    bool hasAutoAnnotations  , 
			
			// const char* leadingSpaces
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string leadingSpaces  , 
			
			// const char* indentSpaces
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string indentSpaces  
			
		);
	}
}