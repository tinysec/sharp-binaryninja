using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTransformSessionProcess(BNTransformSession* session)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTransformSessionProcess"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTransformSessionProcess(
			
			// BNTransformSession* session
		    IntPtr session  
			
		);
	}
}