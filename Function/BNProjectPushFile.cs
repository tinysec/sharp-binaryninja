using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectPushFile(BNProject* project, BNProjectFile* file)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectPushFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNProjectPushFile(
			
			// BNProject* project
		    IntPtr project  , 
			
			// BNProjectFile* file
		    IntPtr file  
			
		);
	}
}