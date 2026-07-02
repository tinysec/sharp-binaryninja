using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNSymbol* BNCreateSymbol(BNSymbolType type, const char* shortName, const char* fullName, const char* rawName, uint64_t addr, BNSymbolBinding binding, BNNameSpace* nameSpace, uint64_t ordinal)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateSymbol"
        )]
		internal static extern IntPtr BNCreateSymbol(
			
			// BNSymbolType type
		    SymbolType type  , 
			
			// const char* shortName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string shortName  , 
			
			// const char* fullName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fullName  , 
			
			// const char* rawName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string rawName  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// BNSymbolBinding binding
		    SymbolBinding binding  , 
			
			// BNNameSpace* _nameSpace
		    IntPtr _nameSpace  , 
			
			// uint64_t ordinal
		    ulong ordinal  
		);
	}
}