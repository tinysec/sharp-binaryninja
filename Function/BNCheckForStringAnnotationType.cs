using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCheckForStringAnnotationType(BNBinaryView* view, uint64_t addr, const char** value, BNStringType* strType, bool allowShortStrings, bool allowLargeStrings, uint64_t childWidth)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCheckForStringAnnotationType"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCheckForStringAnnotationType(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// char** value
		    out IntPtr value  , 
			
			// BNStringType* strType
		    out StringType strType  , 
			
			// bool allowShortStrings
		    bool allowShortStrings  , 
			
			// bool allowLargeStrings
		    bool allowLargeStrings  , 
			
			// uint64_t childWidth
		    ulong childWidth  
		);
	}
}