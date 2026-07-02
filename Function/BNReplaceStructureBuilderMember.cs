using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNReplaceStructureBuilderMember(BNStructureBuilder* s, uint64_t idx, BNTypeWithConfidence* type, const char* name, bool overwriteExisting)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNReplaceStructureBuilderMember"
        )]
		internal static extern void BNReplaceStructureBuilderMember(
			
			// BNStructureBuilder* s
		    IntPtr s  , 
			
			// uint64_t idx
		    ulong idx  , 
			
			// BNTypeWithConfidence* type
			in BNTypeWithConfidence type  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// bool overwriteExisting
		    bool overwriteExisting  
		);
	}
}