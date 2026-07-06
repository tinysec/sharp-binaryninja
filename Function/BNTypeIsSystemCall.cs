using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeIsSystemCall(BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeIsSystemCall"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeIsSystemCall(
			
			// BNType* type
		    IntPtr type  
		);
	}
}