using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSaveToFile(BNBinaryView* view, BNFileAccessor* file)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSaveToFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSaveToFile(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNFileAccessor* file
		    in BNFileAccessor file  
		);
	}
}