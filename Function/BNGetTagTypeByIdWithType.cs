using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTagType* BNGetTagTypeByIdWithType(BNBinaryView* view, const char* id, BNTagTypeType type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTagTypeByIdWithType"
        )]
		internal static extern IntPtr BNGetTagTypeByIdWithType(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  , 
			
			// BNTagTypeType type
		    TagTypeType type  
			
		);
	}
}