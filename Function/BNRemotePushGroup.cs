using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemotePushGroup(BNRemote* remote, BNCollaborationGroup* group, const char** extraFieldKeys, const char** extraFieldValues, uint64_t extraFieldCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemotePushGroup"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemotePushGroup(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// BNCollaborationGroup* _group
		    IntPtr _group  , 
			
			// const char** extraFieldKeys
		    string[] extraFieldKeys  , 
			
			// const char** extraFieldValues
		    string[] extraFieldValues  , 
			
			// uint64_t extraFieldCount
		    ulong extraFieldCount  
			
		);
	}
}