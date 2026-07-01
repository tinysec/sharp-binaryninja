using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNValueLocation BNGetReturnValueLocation( BNCallingConvention* cc, BNBinaryView* view, BNReturnValue* returnValue)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetReturnValueLocation"
        )]
		internal static extern BNValueLocation BNGetReturnValueLocation(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// BNReturnValue* returnValue
		    IntPtr returnValue  
		);
	}
}
