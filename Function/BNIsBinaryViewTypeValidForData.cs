using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsBinaryViewTypeValidForData(BNBinaryViewType* type, BNBinaryView* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsBinaryViewTypeValidForData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsBinaryViewTypeValidForData(
			
			// BNBinaryViewType* type
		    IntPtr type  , 
			
			// BNBinaryView* data
		    IntPtr data  
			
		);
	}
}