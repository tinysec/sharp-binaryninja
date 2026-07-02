using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// uint64_t BNBinaryViewGetAssociatedTypesFromArchive(BNBinaryView* view, const char* archiveId, const char*** typeIds, const char*** archiveTypeIds)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewGetAssociatedTypesFromArchive"
        )]
		internal static extern ulong BNBinaryViewGetAssociatedTypesFromArchive(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* archiveId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string archiveId  , 
			
			// const char*** typeIds
		    IntPtr typeIds  , 
			
			// const char*** archiveTypeIds
		    IntPtr archiveTypeIds  
		);
	}
}