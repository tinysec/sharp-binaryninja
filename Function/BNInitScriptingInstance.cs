using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNScriptingInstance* BNInitScriptingInstance(BNScriptingProvider* provider, BNScriptingInstanceCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNInitScriptingInstance"
        )]
		internal static extern IntPtr BNInitScriptingInstance(
			
			// BNScriptingProvider* provider
		    IntPtr provider  , 
			
			// BNScriptingInstanceCallbacks* callbacks
		    in BNScriptingInstanceCallbacks callbacks
			
		);
	}
}
