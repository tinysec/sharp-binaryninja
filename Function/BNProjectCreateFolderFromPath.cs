using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNProjectFolder* BNProjectCreateFolderFromPath(BNProject* project, const char* path, BNProjectFolder* parent, const char* description, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectCreateFolderFromPath"
        )]
		internal static extern IntPtr BNProjectCreateFolderFromPath(
			
			// BNProject* project
		    IntPtr project  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  , 
			
			// BNProjectFolder* parent
		    IntPtr parent  , 
			
			// const char* description
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string description  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  
			
		);
	}
}