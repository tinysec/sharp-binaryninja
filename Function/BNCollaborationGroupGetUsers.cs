using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNCollaborationUser** BNCollaborationGroupGetUsers(BNCollaborationGroup* group, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationGroupGetUsers"
        )]
		internal static extern IntPtr BNCollaborationGroupGetUsers(
			
			// BNCollaborationGroup* group
		    IntPtr group   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
