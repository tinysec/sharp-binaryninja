using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDemanglerDemangle(BNDemangler* demangler, BNArchitecture* arch, const char* name, BNType** outType, BNQualifiedName* outVarName, BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDemanglerDemangle"
        )]
		internal static extern bool BNDemanglerDemangle(
			
			// BNDemangler* demangler
		    IntPtr demangler  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNType** outType
		    IntPtr outType  , 
			
			// BNQualifiedName* outVarName
		    IntPtr outVarName  , 
			
			// BNBinaryView* view
		    IntPtr view  
			
		);
	}
}