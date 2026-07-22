using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNExecuteOnMainThreadAndWait(void* ctxt, void (*func)(void* ctxt))
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNExecuteOnMainThreadAndWait"
        )]
		internal static extern void BNExecuteOnMainThreadAndWait(
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void (*func)(void* ctxt)
		    IntPtr func  
			
		);
	}
}
