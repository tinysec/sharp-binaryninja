using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeDerivedStringList(BNDerivedString* strings, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeDerivedStringList"
        )]
		internal static extern void BNFreeDerivedStringList(
			
			// BNDerivedString* strings
		    IntPtr strings   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
