using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNReferenceSource* BNGetDerivedStringCodeReferences( BNBinaryView* view, BNDerivedString* str, size_t* count, bool limit, size_t maxItems)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDerivedStringCodeReferences"
        )]
		internal static extern IntPtr BNGetDerivedStringCodeReferences(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// BNDerivedString* str
		    IntPtr str   , 
			
			// size_t* count
		    out UIntPtr count,
			
			// bool limit
		    [MarshalAs(UnmanagedType.I1)] bool limit,
			
			// size_t maxItems
		    UIntPtr maxItems  
		);
	}
}
