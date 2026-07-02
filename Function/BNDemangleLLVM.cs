using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDemangleLLVM(const char* mangledName, const char*** outVarName, uint64_t* outVarNameElements, bool simplify)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDemangleLLVM"
        )]
		internal static extern bool BNDemangleLLVM(
			
			// const char* mangledName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string mangledName  , 
			
			// const char*** outVarName
		    IntPtr outVarName  , 
			
			// uint64_t* outVarNameElements
		    IntPtr outVarNameElements  , 
			
			// bool simplify
		    bool simplify  
			
		);
	}
}