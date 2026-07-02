using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDemangleGeneric(BNArchitecture* arch, const char* name, BNType** outType, BNQualifiedName* outVarName, BNBinaryView* view, bool simplify)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDemangleGeneric"
        )]
		internal static extern bool BNDemangleGeneric(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNType** outType
		    IntPtr outType  , 
			
			// BNQualifiedName* outVarName
		    IntPtr outVarName  , 
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// bool simplify
		    bool simplify  
			
		);
	}
}