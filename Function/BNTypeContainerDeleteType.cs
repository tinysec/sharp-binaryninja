using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerDeleteType(BNTypeContainer* container, const char* typeId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeContainerDeleteType"
        )]
		internal static extern bool BNTypeContainerDeleteType(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// const char* typeId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeId  
		);
	}
}