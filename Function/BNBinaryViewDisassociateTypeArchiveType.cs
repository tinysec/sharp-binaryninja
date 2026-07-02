using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewDisassociateTypeArchiveType(BNBinaryView* view, const char* typeId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewDisassociateTypeArchiveType"
        )]
		internal static extern bool BNBinaryViewDisassociateTypeArchiveType(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* typeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeId  
		);
	}
}