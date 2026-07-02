using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationUser* BNRemoteCreateUser(BNRemote* remote, const char* username, const char* email, bool isActive, const char* password, uint64_t* groupIds, uint64_t groupIdCount, uint64_t* userPermissionIds, uint64_t userPermissionIdCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteCreateUser"
        )]
		internal static extern IntPtr BNRemoteCreateUser(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  , 
			
			// const char* email
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string email  , 
			
			// bool isActive
		    bool isActive  , 
			
			// const char* password
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string password  , 
			
			// uint64_t* groupIds
		    IntPtr groupIds  , 
			
			// uint64_t groupIdCount
		    ulong groupIdCount  , 
			
			// uint64_t* userPermissionIds
		    IntPtr userPermissionIds  , 
			
			// uint64_t userPermissionIdCount
		    ulong userPermissionIdCount  
			
		);
	}
}