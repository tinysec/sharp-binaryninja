using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFindAllTextWithProgress(BNBinaryView* view, uint64_t start, uint64_t end, const char* data, BNDisassemblySettings* settings, BNFindFlag flags, BNFunctionViewType viewType, void* ctxt, void** progress, void* matchCtxt, void** matchCallback)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFindAllTextWithProgress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFindAllTextWithProgress(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t start
		    ulong start  , 
			
			// uint64_t end
		    ulong end  , 
			
			// const char* data
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string data  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// BNFindFlag flags
		    FindFlag flags  , 
			
			// BNFunctionViewType viewType
		    in BNFunctionViewType viewType  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* matchCtxt
		    IntPtr matchCtxt  , 
			
			// void** matchCallback
		    IntPtr matchCallback  
		);
	}
}