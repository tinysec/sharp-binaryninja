using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDemangleMSPlatform(BNPlatform* platform, const char* mangledName, BNType** outType, const char*** outVarName, uint64_t* outVarNameElements, bool simplify)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDemangleMSPlatform"
        )]
		internal static extern bool BNDemangleMSPlatform(
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// const char* mangledName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string mangledName  , 
			
			// BNType** outType
		    IntPtr outType  , 
			
			// const char*** outVarName
		    IntPtr outVarName  , 
			
			// uint64_t* outVarNameElements
		    IntPtr outVarNameElements  , 
			
			// bool simplify
		    bool simplify  
			
		);
	}
}