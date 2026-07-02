using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationUserSetUsername(BNCollaborationUser* user, const char* username)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationUserSetUsername"
        )]
		internal static extern bool BNCollaborationUserSetUsername(
			
			// BNCollaborationUser* user
		    IntPtr user  , 
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  
		);
	}
}