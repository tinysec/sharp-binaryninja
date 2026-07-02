using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNBinaryViewStoreMetadata(BNBinaryView* view, const char* key, BNMetadata* value, bool isAuto)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewStoreMetadata"
        )]
		internal static extern void BNBinaryViewStoreMetadata(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNMetadata* value
		    IntPtr value  , 
			
			// bool isAuto
		    bool isAuto  
		);
	}
}