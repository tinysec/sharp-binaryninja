using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNProjectFile* BNProjectCreateFile(BNProject* project, uint8_t* contents, uint64_t contentsSize, BNProjectFolder* folder, const char* name, const char* description, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectCreateFile"
        )]
		internal static extern IntPtr BNProjectCreateFile(
			
			// BNProject* project
		    IntPtr project  , 
			
			// uint8_t* contents
		    IntPtr contents  , 
			
			// uint64_t contentsSize
		    ulong contentsSize  , 
			
			// BNProjectFolder* folder
		    IntPtr folder  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* description
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string description  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  
			
		);
	}
}