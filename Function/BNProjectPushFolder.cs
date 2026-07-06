using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectPushFolder(BNProject* project, BNProjectFolder* folder)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectPushFolder"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectPushFolder(
			
			// BNProject* project
		    IntPtr project  , 
			
			// BNProjectFolder* folder
		    IntPtr folder  
			
		);
	}
}