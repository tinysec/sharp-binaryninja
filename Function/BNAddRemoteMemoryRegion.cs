using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddRemoteMemoryRegion(BNBinaryView* view, const char* name, uint64_t start, BNFileAccessor* accessor, uint32_t flags)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddRemoteMemoryRegion"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAddRemoteMemoryRegion(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t start
		    ulong start  , 
			
			// BNFileAccessor* accessor
		    in BNFileAccessor accessor  , 
			
			// uint32_t flags
		    uint flags  
		);
	}
}