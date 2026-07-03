using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNLlvmServicesAssembleFree(const char* outBytes, const char* err)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLlvmServicesAssembleFree"
        )]
		internal static extern void BNLlvmServicesAssembleFree(
			
			// char* outBytes: the raw pointer returned by BNLlvmServicesAssemble (pass
			// it back verbatim to free it; must NOT be re-marshaled from a managed string).
		    IntPtr outBytes  ,

			// char* err: the raw pointer returned by BNLlvmServicesAssemble.
		    IntPtr err
			
		);
	}
}