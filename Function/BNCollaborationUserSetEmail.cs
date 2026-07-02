using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationUserSetEmail(BNCollaborationUser* user, const char* email)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationUserSetEmail"
        )]
		internal static extern bool BNCollaborationUserSetEmail(
			
			// BNCollaborationUser* user
		    IntPtr user  , 
			
			// const char* email
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string email  
		);
	}
}