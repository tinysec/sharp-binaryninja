using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNMetadata* BNCreateMetadataStringListData(const char** data, uint64_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateMetadataStringListData"
        )]
		internal static extern IntPtr BNCreateMetadataStringListData(

			// const char** data (UTF-8 block built by the caller; string[] cannot be
			// marshaled as UTF-8 by attribute, so it is passed as a raw pointer)
		    IntPtr data  ,

			// uint64_t size
		    ulong size
		);
	}
}