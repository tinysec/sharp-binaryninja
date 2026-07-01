using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNAddUserSections(BNBinaryView* view, const BNSectionInfo* sectionInfo, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAddUserSections"
        )]
		internal static extern void BNAddUserSections(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// const BNSectionInfo* sectionInfo
		    IntPtr sectionInfo   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
