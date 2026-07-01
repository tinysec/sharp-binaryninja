using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNRepository** BNRepositoryManagerGetRepositories(size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRepositoryManagerGetRepositories"
        )]
		internal static extern IntPtr BNRepositoryManagerGetRepositories(
			
			// size_t* count
		    IntPtr count  
		);
	}
}
