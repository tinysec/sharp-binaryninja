using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNParseTypesFromSourceFile(BNPlatform* platform, const char* fileName, BNTypeParserResult* result, const char** errors, const char** includeDirs, uint64_t includeDirCount, const char* autoTypeSource)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParseTypesFromSourceFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNParseTypesFromSourceFile(
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// const char* fileName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fileName  , 
			
			// BNTypeParserResult* result
		    IntPtr result  ,

			// char** errors: a SINGLE error string the core allocates on failure;
			// marshaled as an out pointer so the wrapper can decode + free it.
		    out IntPtr errors  ,

			// const char** includeDirs: caller-built UTF-8 char** block.
		    IntPtr includeDirs  ,

			// uint64_t includeDirCount
		    ulong includeDirCount  ,
			
			// const char* autoTypeSource
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string autoTypeSource  
			
		);
	}
}