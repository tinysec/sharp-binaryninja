using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNComponent* BNCreateComponentWithParent(BNBinaryView* view, const char* parentGUID)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateComponentWithParent"
        )]
		internal static extern IntPtr BNCreateComponentWithParent(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* parentGUID
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string parentGUID  
		);
	}
}