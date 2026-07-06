using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNParseTypeString(BNBinaryView* view, const char* text, BNQualifiedNameAndType* result, const char** errors, BNQualifiedNameList* typesAllowRedefinition, bool importDepencencies)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParseTypeString"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNParseTypeString(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* text
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string text  , 
			
			// BNQualifiedNameAndType* result
		    out BNQualifiedNameAndType result  , 
			
			// char** errors
		    out IntPtr errors  , 
			
			// BNQualifiedNameList* typesAllowRedefinition
			in BNQualifiedNameList typesAllowRedefinition  , 
			
			// bool importDepencencies
		    bool importDepencencies  
		);
	}
}