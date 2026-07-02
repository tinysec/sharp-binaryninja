using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBinaryViewType* BNRegisterBinaryViewType(const char* name, const char* longName, BNCustomBinaryViewType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterBinaryViewType"
        )]
		internal static extern IntPtr BNRegisterBinaryViewType(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* longName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string longName  , 
			
			// BNCustomBinaryViewType* type
		    IntPtr type  
		);
	}
}