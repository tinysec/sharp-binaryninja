using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNSymbol** BNGetSymbolsByRawName(BNBinaryView* view, const char* name, uint64_t* count, BNNameSpace* nameSpace)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetSymbolsByRawName"
        )]
		internal static extern IntPtr BNGetSymbolsByRawName(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t* count
		    out ulong count  , 
			
			// BNNameSpace* _nameSpace
		    IntPtr _nameSpace
		);
	}
}