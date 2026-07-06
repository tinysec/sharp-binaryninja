using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTransformSessionProcessFrom(BNTransformSession* session, BNTransformContext* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTransformSessionProcessFrom"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTransformSessionProcessFrom(
			
			// BNTransformSession* session
		    IntPtr session  , 
			
			// BNTransformContext* context
		    IntPtr context  
			
		);
	}
}