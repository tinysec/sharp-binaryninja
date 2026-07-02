using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerParseTypeString(BNTypeContainer* container, const char* source, bool importDepencencies, BNQualifiedNameAndType* result, BNTypeParserError** errors, uint64_t* errorCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeContainerParseTypeString"
        )]
		internal static extern bool BNTypeContainerParseTypeString(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  , 
			
			// bool importDepencencies
		    bool importDepencencies  , 
			
			// BNQualifiedNameAndType* result
			out BNQualifiedNameAndType result  , 
			
			// BNTypeParserError** errors
			out IntPtr errors  , 
			
			// uint64_t* errorCount
		    out ulong errorCount  
		);
	}
}