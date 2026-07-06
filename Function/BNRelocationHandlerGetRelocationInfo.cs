using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRelocationHandlerGetRelocationInfo(BNRelocationHandler* handler, BNBinaryView* data, BNArchitecture* arch, BNRelocationInfo* info, uint64_t infoCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRelocationHandlerGetRelocationInfo"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRelocationHandlerGetRelocationInfo(
			
			// BNRelocationHandler* handler
		    IntPtr handler  , 
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// BNRelocationInfo* info
		    IntPtr info  , 
			
			// uint64_t infoCount
		    ulong infoCount  
			
		);
	}
}