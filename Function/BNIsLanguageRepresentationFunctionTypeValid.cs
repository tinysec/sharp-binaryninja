using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsLanguageRepresentationFunctionTypeValid(BNLanguageRepresentationFunctionType* type, BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsLanguageRepresentationFunctionTypeValid"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsLanguageRepresentationFunctionTypeValid(
			
			// BNLanguageRepresentationFunctionType* type
		    IntPtr type  , 
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}