using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNCollaborationGroupSetUsers(BNCollaborationGroup* group, BNCollaborationUser** users, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationGroupSetUsers"
        )]
		internal static extern bool BNCollaborationGroupSetUsers(
			
			// BNCollaborationGroup* group
		    IntPtr group   , 
			
			// BNCollaborationUser** users
		    IntPtr users   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
