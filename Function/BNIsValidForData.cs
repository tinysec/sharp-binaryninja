using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsValidForData(void* ctxt, BNBinaryView* view, uint64_t addr, BNType* type, BNTypeContext* typeCtx, uint64_t ctxCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsValidForData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsValidForData(
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// BNType* type
		    IntPtr type  , 
			
			// BNTypeContext* typeCtx
		    IntPtr typeCtx  , 
			
			// uint64_t ctxCount
		    ulong ctxCount  
			
		);
	}
}