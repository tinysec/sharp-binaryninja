using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNMainThreadAction* BNExecuteOnMainThread(void* ctxt, void (*func)(void* ctxt))
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNExecuteOnMainThread"
        )]
		internal static extern IntPtr BNExecuteOnMainThread(
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void (*func)(void* ctxt)
		    IntPtr func  
			
		);
	}
}
