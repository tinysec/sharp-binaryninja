using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNProjectFileRemoveDependency(BNProjectFile* file, BNProjectFile* dep)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNProjectFileRemoveDependency"
        )]
		internal static extern bool BNProjectFileRemoveDependency(
			
			// BNProjectFile* file
		    IntPtr file   , 
			
			// BNProjectFile* dep
		    IntPtr dep  
		);
	}
}
