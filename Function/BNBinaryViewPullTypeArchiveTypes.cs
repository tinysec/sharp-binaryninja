using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewPullTypeArchiveTypes(BNBinaryView* view, const char* archiveId, const char** archiveTypeIds, uint64_t archiveTypeIdCount, const char*** updatedArchiveTypeIds, const char*** updatedAnalysisTypeIds, uint64_t* updatedTypeCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewPullTypeArchiveTypes"
        )]
		internal static extern bool BNBinaryViewPullTypeArchiveTypes(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* archiveId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string archiveId  , 
			
			// const char** archiveTypeIds
		    string[] archiveTypeIds  , 
			
			// uint64_t archiveTypeIdCount
		    ulong archiveTypeIdCount  , 
			
			// const char*** updatedArchiveTypeIds
		    IntPtr updatedArchiveTypeIds  , 
			
			// const char*** updatedAnalysisTypeIds
		    IntPtr updatedAnalysisTypeIds  , 
			
			// uint64_t* updatedTypeCount
		    IntPtr updatedTypeCount  
		);
	}
}