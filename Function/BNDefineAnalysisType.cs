using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNQualifiedName BNDefineAnalysisType(BNBinaryView* view, const char* id, BNQualifiedName* defaultName, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDefineAnalysisType"
        )]
		internal static extern BNQualifiedName BNDefineAnalysisType(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  , 
			
			// BNQualifiedName* defaultName
		    in BNQualifiedName defaultName  , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}