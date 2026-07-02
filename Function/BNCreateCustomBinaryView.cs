using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBinaryView* BNCreateCustomBinaryView(const char* name, BNFileMetadata* file, BNBinaryView* parent, BNCustomBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateCustomBinaryView"
        )]
		internal static extern IntPtr BNCreateCustomBinaryView(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// BNBinaryView* parent
		    IntPtr parent  , 
			
			// BNCustomBinaryView* view
		    in BNCustomBinaryView view  
		);
	}
}