using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeBuilder* BNCreateIntegerTypeBuilder(uint64_t width, BNBoolWithConfidence* sign, const char* altName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateIntegerTypeBuilder"
        )]
		internal static extern IntPtr BNCreateIntegerTypeBuilder(
			
			// uint64_t width
		    ulong width  , 
			
			// BNBoolWithConfidence* sign
			in BNBoolWithConfidence sign  , 
			
			// const char* altName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string altName  
		);
	}
}