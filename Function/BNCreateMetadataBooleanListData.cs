using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNMetadata* BNCreateMetadataBooleanListData(bool* data, uint64_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateMetadataBooleanListData"
        )]
		internal static extern IntPtr BNCreateMetadataBooleanListData(
			
			// bool* data
			// Native bool is 1 byte; the default managed bool[] marshals as 4-byte Win32 BOOL,
			// which mis-packs the array. Marshal each element as a single byte.
		    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] bool[] data  ,
			
			// uint64_t size
		    ulong size  
		);
	}
}