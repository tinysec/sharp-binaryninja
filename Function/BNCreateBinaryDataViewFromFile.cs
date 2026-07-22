using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBinaryView* BNCreateBinaryDataViewFromFile(BNFileMetadata* file, BNFileAccessor* accessor)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateBinaryDataViewFromFile"
        )]
		internal static extern IntPtr BNCreateBinaryDataViewFromFile(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// BNFileAccessor* accessor
		    in BNFileAccessor accessor
		);
	}
}
