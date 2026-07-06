using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteProjectPushFile(BNRemoteProject* project, BNRemoteFile* file, const char** extraFieldKeys, const char** extraFieldValues, uint64_t extraFieldCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectPushFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteProjectPushFile(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// const char** extraFieldKeys
		    string[] extraFieldKeys  , 
			
			// const char** extraFieldValues
		    string[] extraFieldValues  , 
			
			// uint64_t extraFieldCount
		    ulong extraFieldCount  
			
		);
	}
}