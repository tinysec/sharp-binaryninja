using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationStoreDataInKeychain(const char* key, const char** dataKeys, const char** dataValues, uint64_t dataCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationStoreDataInKeychain"
        )]
		internal static extern bool BNCollaborationStoreDataInKeychain(
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char** dataKeys
		    string[] dataKeys  , 
			
			// const char** dataValues
		    string[] dataValues  , 
			
			// uint64_t dataCount
		    ulong dataCount  
		);
	}
}