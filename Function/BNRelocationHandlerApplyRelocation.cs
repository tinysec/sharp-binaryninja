using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRelocationHandlerApplyRelocation(BNRelocationHandler* handler, BNBinaryView* view, BNArchitecture* arch, BNRelocation* reloc, uint8_t* dest, uint64_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRelocationHandlerApplyRelocation"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRelocationHandlerApplyRelocation(
			
			// BNRelocationHandler* handler
		    IntPtr handler  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// BNRelocation* reloc
		    IntPtr reloc  , 
			
			// uint8_t* dest
		    IntPtr dest  , 
			
			// uint64_t len
		    ulong len  
			
		);
	}
}