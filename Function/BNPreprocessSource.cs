using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNPreprocessSource(const char* source, const char* fileName, const char** output, const char** errors, const char** includeDirs, uint64_t includeDirCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPreprocessSource"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNPreprocessSource(
			
			// const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  , 
			
			// const char* fileName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fileName  , 
			
			// char** output: a SINGLE preprocessed-source string the core allocates;
			// marshaled as an out pointer so the wrapper can decode + free it.
		    out IntPtr output  ,

			// char** errors: a SINGLE error string the core allocates on failure.
		    out IntPtr errors  ,

			// const char** includeDirs: caller-built UTF-8 char** block.
		    IntPtr includeDirs  ,

			// uint64_t includeDirCount
		    ulong includeDirCount
			
		);
	}
}