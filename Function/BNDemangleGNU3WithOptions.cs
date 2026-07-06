using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDemangleGNU3WithOptions(BNArchitecture* arch, const char* mangledName, BNType** outType, const char*** outVarName, uint64_t* outVarNameElements, BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDemangleGNU3WithOptions"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNDemangleGNU3WithOptions(
			
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