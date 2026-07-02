using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNTagTypeSetIcon(BNTagType* tagType, const char* icon)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTagTypeSetIcon"
        )]
		internal static extern void BNTagTypeSetIcon(
			
			// BNTagType* tagType
		    IntPtr tagType  , 
			
			// const char* icon
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string icon  
		);
	}
}