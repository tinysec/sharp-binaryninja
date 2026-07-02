using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddStructureBuilderMemberAtOffset(BNStructureBuilder* s, BNTypeWithConfidence* type, const char* name, uint64_t offset, bool overwriteExisting, BNMemberAccess access, BNMemberScope scope, uint8_t bitPosition, uint8_t bitWidth)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddStructureBuilderMemberAtOffset"
        )]
		internal static extern void BNAddStructureBuilderMemberAtOffset(
			
			// BNStructureBuilder* s
		    IntPtr s  , 
			
			// BNTypeWithConfidence* type
			in BNTypeWithConfidence type  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t offset
		    ulong offset  , 
			
			// bool overwriteExisting
		    bool overwriteExisting  , 
			
			// BNMemberAccess access
		    MemberAccess access  , 
			
			// BNMemberScope scope
		    MemberScope scope  ,

			// uint8_t bitPosition
		    byte bitPosition  ,

			// uint8_t bitWidth
		    byte bitWidth
		);
	}
}