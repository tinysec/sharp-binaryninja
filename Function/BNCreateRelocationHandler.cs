using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRelocationHandler* BNCreateRelocationHandler(BNCustomRelocationHandler* handler)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateRelocationHandler"
        )]
		internal static extern IntPtr BNCreateRelocationHandler(
			in BNCustomRelocationHandler handler
		);
	}
}
