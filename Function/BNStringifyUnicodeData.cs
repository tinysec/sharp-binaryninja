using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNStringifyUnicodeData(BNBinaryView* data, BNArchitecture* arch, BNDataBuffer* buffer, bool nullTerminates, bool allowShortStrings, const char** @string, BNStringType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNStringifyUnicodeData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNStringifyUnicodeData(
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// BNDataBuffer* buffer
		    IntPtr buffer  , 
			
			// bool nullTerminates
		    bool nullTerminates  , 
			
			// bool allowShortStrings
		    bool allowShortStrings  , 
			
			// char** text
		    out IntPtr text  , 
			
			// BNStringType* type
			out StringType type  
		);
	}
}