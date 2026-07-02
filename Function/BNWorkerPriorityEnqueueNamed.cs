using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNWorkerPriorityEnqueueNamed(void* ctxt, void** action, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWorkerPriorityEnqueueNamed"
        )]
		internal static extern void BNWorkerPriorityEnqueueNamed(
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** action
		    IntPtr action  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
			
		);
	}
}