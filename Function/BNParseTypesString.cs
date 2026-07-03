using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNParseTypesString(BNBinaryView* view, const char* text, const char** options, uint64_t optionCount, const char** includeDirs, uint64_t includeDirCount, BNTypeParserResult* result, const char** errors, BNQualifiedNameList* typesAllowRedefinition, bool importDepencencies)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParseTypesString"
        )]
		internal static extern bool BNParseTypesString(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* text
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string text  , 
			
			// const char** options: caller-built UTF-8 char** block.
		    IntPtr options  ,

			// uint64_t optionCount
		    ulong optionCount  ,

			// const char** includeDirs: caller-built UTF-8 char** block.
		    IntPtr includeDirs  ,

			// uint64_t includeDirCount
		    ulong includeDirCount  ,
			
			// BNTypeParserResult* result
		    out BNTypeParserResult result  , 
			
			// char** errors
		    out IntPtr errors  , 
			
			// BNQualifiedNameList* typesAllowRedefinition
		    in BNQualifiedNameList typesAllowRedefinition  , 
			
			// bool importDepencencies
		    bool importDepencencies  
		);
	}
}