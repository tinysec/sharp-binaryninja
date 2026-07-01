using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNProjectFileAddDependency(BNProjectFile* file, BNProjectFile* dep)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNProjectFileAddDependency"
        )]
		internal static extern bool BNProjectFileAddDependency(
			
			// BNProjectFile* file
		    IntPtr file   , 
			
			// BNProjectFile* dep
		    IntPtr dep  
		);
	}
}
