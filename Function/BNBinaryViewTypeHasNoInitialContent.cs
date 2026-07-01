using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNBinaryViewTypeHasNoInitialContent(BNBinaryViewType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBinaryViewTypeHasNoInitialContent"
        )]
		internal static extern bool BNBinaryViewTypeHasNoInitialContent(
			
			// BNBinaryViewType* type
		    IntPtr type  
		);
	}
}
