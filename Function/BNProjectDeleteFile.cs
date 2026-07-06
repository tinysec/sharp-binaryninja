using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectDeleteFile(BNProject* project, BNProjectFile* file)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectDeleteFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectDeleteFile(
			
			// BNProject* project
		    IntPtr project  , 
			
			// BNProjectFile* file
		    IntPtr file  
			
		);
	}
}