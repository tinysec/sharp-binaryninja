using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddStructureBuilderMember(BNStructureBuilder* s, BNTypeWithConfidence* type, const char* name, BNMemberAccess access, BNMemberScope scope)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddStructureBuilderMember"
        )]
		internal static extern void BNAddStructureBuilderMember(
			
			// BNStructureBuilder* s
		    IntPtr s  , 
			
			// BNTypeWithConfidence* type
			in BNTypeWithConfidence type  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNMemberAccess access
		    MemberAccess access  , 
			
			// BNMemberScope scope
		    MemberScope scope  
		);
	}
}