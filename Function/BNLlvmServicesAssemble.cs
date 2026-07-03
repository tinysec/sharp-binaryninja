using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// int32_t BNLlvmServicesAssemble(const char* src, int32_t dialect, const char* triplet, int32_t codeModel, int32_t relocMode, const char** outBytes, int32_t* outBytesLen, const char** err, int32_t* errLen)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLlvmServicesAssemble"
        )]
		internal static extern int BNLlvmServicesAssemble(
			
			// const char* src
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string src  , 
			
			// int32_t dialect
		    int dialect  , 
			
			// const char* triplet
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string triplet  , 
			
			// int32_t codeModel
		    int codeModel  , 
			
			// int32_t relocMode
		    int relocMode  , 
			
			// char** outBytes: core-allocated length-delimited byte buffer (NOT a
			// null-terminated string); freed by BNLlvmServicesAssembleFree.
		    out IntPtr outBytes  ,

			// int32_t* outBytesLen
		    IntPtr outBytesLen  ,

			// char** err: core-allocated error string; freed by BNLlvmServicesAssembleFree.
		    out IntPtr err  ,

			// int32_t* errLen
		    IntPtr errLen
			
		);
	}
}