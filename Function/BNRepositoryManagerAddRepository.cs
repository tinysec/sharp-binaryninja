using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNRepositoryManagerAddRepository(const char* url, const char* repoPath)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRepositoryManagerAddRepository"
        )]
		internal static extern bool BNRepositoryManagerAddRepository(
			
			// const char* url
		    string url   , 
			
			// const char* repoPath
		    string repoPath  
		);
	}
}
