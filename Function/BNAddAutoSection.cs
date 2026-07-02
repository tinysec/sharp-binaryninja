using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddAutoSection(BNBinaryView* view, const char* name, uint64_t start, uint64_t length, BNSectionSemantics semantics, const char* type, uint64_t align, uint64_t entrySize, const char* linkedSection, const char* infoSection, uint64_t infoData)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddAutoSection"
        )]
		internal static extern void BNAddAutoSection(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t start
		    ulong start  , 
			
			// uint64_t length
		    ulong length  , 
			
			// BNSectionSemantics semantics
		    SectionSemantics semantics  , 
			
			// const char* type
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string type  , 
			
			// uint64_t align
		    ulong align  , 
			
			// uint64_t entrySize
		    ulong entrySize  , 
			
			// const char* linkedSection
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string linkedSection  , 
			
			// const char* infoSection
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string infoSection  , 
			
			// uint64_t infoData
		    ulong infoData  
		);
	}
}