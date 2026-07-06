using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerParseTypesFromSource(BNTypeContainer* container, const char* source, const char* fileName, const char** options, uint64_t optionCount, const char** includeDirs, uint64_t includeDirCount, const char* autoTypeSource, bool importDepencencies, BNTypeParserResult* result, BNTypeParserError** errors, uint64_t* errorCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeContainerParseTypesFromSource"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerParseTypesFromSource(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  , 
			
			// const char* fileName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fileName  , 
			
			// const char** options: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr options  ,

			// uint64_t optionCount
		    ulong optionCount  ,

			// const char** includeDirs: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr includeDirs  ,

			// uint64_t includeDirCount
		    ulong includeDirCount  ,
			
			// const char* autoTypeSource
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string autoTypeSource  , 
			
			// bool importDepencencies
		    bool importDepencencies  , 
			
			// BNTypeParserResult* result
		    IntPtr result  , 
			
			// BNTypeParserError** errors
		    IntPtr errors  , 
			
			// uint64_t* errorCount
		    IntPtr errorCount  
			
		);
	}
}