using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewLookupImportedObjectLibrary(BNBinaryView* view, BNPlatform* tgtPlatform, uint64_t tgtAddr, BNTypeLibrary** lib, BNQualifiedName* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBinaryViewLookupImportedObjectLibrary"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNBinaryViewLookupImportedObjectLibrary(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNPlatform* tgtPlatform
		    IntPtr tgtPlatform  , 
			
			// uint64_t tgtAddr
		    ulong tgtAddr  , 
			
			// BNTypeLibrary** lib
		    out IntPtr lib  , 
			
			// BNQualifiedName* name
		    out BNQualifiedName name  
		);
	}
}