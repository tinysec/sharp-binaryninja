using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewPushTypeArchiveTypes(BNBinaryView* view, const char* archiveId, const char** typeIds, uint64_t typeIdCount, const char*** updatedAnalysisTypeIds, const char*** updatedArchiveTypeIds, uint64_t* updatedTypeCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewPushTypeArchiveTypes"
        )]
		internal static extern bool BNBinaryViewPushTypeArchiveTypes(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* archiveId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string archiveId  , 
			
			// const char** typeIds
		    string[] typeIds  , 
			
			// uint64_t typeIdCount
		    ulong typeIdCount  , 
			
			// const char*** updatedAnalysisTypeIds
		    IntPtr updatedAnalysisTypeIds  , 
			
			// const char*** updatedArchiveTypeIds
		    IntPtr updatedArchiveTypeIds  , 
			
			// uint64_t* updatedTypeCount
		    IntPtr updatedTypeCount  
		);
	}
}