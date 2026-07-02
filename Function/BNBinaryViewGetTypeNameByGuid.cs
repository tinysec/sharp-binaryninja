using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNQualifiedName BNBinaryViewGetTypeNameByGuid(BNBinaryView* view, const char* guid)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewGetTypeNameByGuid"
        )]
		internal static extern BNQualifiedName BNBinaryViewGetTypeNameByGuid(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* guid
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string guid  
		);
	}
}