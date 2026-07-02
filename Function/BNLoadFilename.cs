using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBinaryView* BNLoadFilename(const char* filename, bool updateAnalysis, const char* options, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLoadFilename"
        )]
		internal static extern IntPtr BNLoadFilename(
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename  , 
			
			// bool updateAnalysis
		    bool updateAnalysis  , 
			
			// const char* options
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string options  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
		);
	}
}