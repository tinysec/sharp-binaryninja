using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNExternalLocation* BNBinaryViewAddExternalLocation(BNBinaryView* view, BNSymbol* sourceSymbol, BNExternalLibrary* library, const char* targetSymbol, uint64_t* targetAddress, bool isAuto)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewAddExternalLocation"
        )]
		internal static extern IntPtr BNBinaryViewAddExternalLocation(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNSymbol* sourceSymbol
		    IntPtr sourceSymbol  , 
			
			// BNExternalLibrary* library
		    IntPtr library  , 
			
			// const char* targetSymbol
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string targetSymbol  , 
			
			// uint64_t* targetAddress
		    IntPtr targetAddress  , 
			
			// bool isAuto
		    bool isAuto  
		);
	}
}