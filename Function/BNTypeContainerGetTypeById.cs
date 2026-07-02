using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypeById(BNTypeContainer* container, const char* typeId, BNType** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeContainerGetTypeById"
        )]
		internal static extern bool BNTypeContainerGetTypeById(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// const char* typeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeId  , 
			
			// BNType** result
		    out IntPtr result  
		);
	}
}