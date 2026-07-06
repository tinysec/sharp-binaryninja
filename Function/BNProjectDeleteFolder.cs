using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectDeleteFolder(BNProject* project, BNProjectFolder* folder, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectDeleteFolder"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectDeleteFolder(
			
			// BNProject* project
		    IntPtr project  , 
			
			// BNProjectFolder* folder
		    IntPtr folder  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  
			
		);
	}
}