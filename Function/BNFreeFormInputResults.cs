using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNFreeFormInputResults(BNFormInputField* fields, uint64_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeFormInputResults"
        )]
		internal static extern void BNFreeFormInputResults(
			
			// BNFormInputField* fields
		    [In] BNFormInputField[] fields  ,
			
			// uint64_t count
		    ulong count  
		);
	}
}
