using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNExternalLibrary* BNBinaryViewAddExternalLibrary(BNBinaryView* view, const char* name, BNProjectFile* backingFile, bool isAuto)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewAddExternalLibrary"
        )]
		internal static extern IntPtr BNBinaryViewAddExternalLibrary(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNProjectFile* backingFile
		    IntPtr backingFile  , 
			
			// bool isAuto
		    bool isAuto  
		);
	}
}