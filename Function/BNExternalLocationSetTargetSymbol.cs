using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNExternalLocationSetTargetSymbol(BNExternalLocation* loc, const char* symbol)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNExternalLocationSetTargetSymbol"
        )]
		internal static extern bool BNExternalLocationSetTargetSymbol(
			
			// BNExternalLocation* loc
		    IntPtr loc  , 
			
			// const char* symbol
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string symbol  
			
		);
	}
}