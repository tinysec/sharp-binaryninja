using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsBackedByDatabase(BNFileMetadata* file, const char* binaryViewType)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsBackedByDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsBackedByDatabase(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// const char* binaryViewType
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string binaryViewType  
		);
	}
}