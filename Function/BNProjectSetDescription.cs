using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNProjectSetDescription(BNProject* project, const char* description)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNProjectSetDescription"
        )]
		internal static extern bool BNProjectSetDescription(
			
			// BNProject* project
		    IntPtr project  , 
			
			// const char* description
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string description  
			
		);
	}
}