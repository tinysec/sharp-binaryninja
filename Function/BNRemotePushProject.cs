using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemotePushProject(BNRemote* remote, BNRemoteProject* project, const char** extraFieldKeys, const char** extraFieldValues, uint64_t extraFieldCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemotePushProject"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemotePushProject(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// const char** extraFieldKeys
		    string[] extraFieldKeys  , 
			
			// const char** extraFieldValues
		    string[] extraFieldValues  , 
			
			// uint64_t extraFieldCount
		    ulong extraFieldCount  
			
		);
	}
}