using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNProjectFile** BNProjectGetFilesByPathInProject(BNProject* project, const char* path, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectGetFilesByPathInProject"
        )]
		internal static extern IntPtr BNProjectGetFilesByPathInProject(
			
			// BNProject* project
		    IntPtr project  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}