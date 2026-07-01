using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNTypeContainerGetTypeCount(BNTypeContainer* container)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeContainerGetTypeCount"
        )]
		internal static extern UIntPtr BNTypeContainerGetTypeCount(
			
			// BNTypeContainer* container
		    IntPtr container  
		);
	}
}
