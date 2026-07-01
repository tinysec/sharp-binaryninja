using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNRelocation* BNGetNextRelocation(BNBinaryView* view, uint64_t addr, uint64_t maxAddr)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetNextRelocation"
        )]
		internal static extern IntPtr BNGetNextRelocation(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// uint64_t addr
		    ulong addr   , 
			
			// uint64_t maxAddr
		    ulong maxAddr  
		);
	}
}
