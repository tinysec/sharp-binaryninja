using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNComponent* BNGetComponentByGuid(BNBinaryView* view, const char* guid)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetComponentByGuid"
        )]
		internal static extern IntPtr BNGetComponentByGuid(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* guid
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string guid  
		);
	}
}