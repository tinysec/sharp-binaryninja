using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNExternalLocationHasTargetSymbol(BNExternalLocation* loc)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNExternalLocationHasTargetSymbol"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNExternalLocationHasTargetSymbol(
			
			// BNExternalLocation* loc
		    IntPtr loc  
			
		);
	}
}