using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFindNextText(BNBinaryView* view, uint64_t start, const char* data, uint64_t* result, BNDisassemblySettings* settings, BNFindFlag flags, BNFunctionViewType viewType)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFindNextText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFindNextText(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t start
		    ulong start  , 
			
			// const char* data
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string data  , 
			
			// uint64_t* result
		    out ulong result  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNFindFlag flags
		    FindFlag flags  , 
			
			// BNFunctionViewType viewType
		    BNFunctionViewType viewType  
		);
	}
}