using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNBinaryViewSetLoadSettings(BNBinaryView* view, const char* typeName, BNSettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewSetLoadSettings"
        )]
		internal static extern void BNBinaryViewSetLoadSettings(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* typeName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeName  , 
			
			// BNSettings* settings
		    IntPtr settings  
		);
	}
}