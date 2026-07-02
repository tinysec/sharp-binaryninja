using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationPermission* BNRemoteProjectCreateUserPermission(BNRemoteProject* project, const char* userId, BNCollaborationPermissionLevel level, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectCreateUserPermission"
        )]
		internal static extern IntPtr BNRemoteProjectCreateUserPermission(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// const char* userId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string userId  , 
			
			// BNCollaborationPermissionLevel level
		    CollaborationPermissionLevel level  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
			
		);
	}
}