using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDemangleMSWithOptions(BNArchitecture* arch, const char* mangledName, BNType** outType, const char*** outVarName, uint64_t* outVarNameElements, BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDemangleMSWithOptions"
        )]
		internal static extern bool BNDemangleMSWithOptions(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* mangledName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string mangledName  , 
			
			// BNType** outType
		    IntPtr outType  , 
			
			// const char*** outVarName
		    IntPtr outVarName  , 
			
			// uint64_t* outVarNameElements
		    IntPtr outVarNameElements  , 
			
			// BNBinaryView* view
		    IntPtr view  
			
		);
	}
}