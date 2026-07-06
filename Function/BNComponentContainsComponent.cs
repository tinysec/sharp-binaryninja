using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNComponentContainsComponent(BNComponent* parent, BNComponent* component)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNComponentContainsComponent"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNComponentContainsComponent(
			
			// BNComponent* parent
		    IntPtr parent  , 
			
			// BNComponent* component
		    IntPtr component  
		);
	}
}