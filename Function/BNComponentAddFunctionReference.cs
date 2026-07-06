using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNComponentAddFunctionReference(BNComponent* component, BNFunction* function)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNComponentAddFunctionReference"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNComponentAddFunctionReference(
			
			// BNComponent* component
		    IntPtr component  , 
			
			// BNFunction* function
		    IntPtr function  
		);
	}
}