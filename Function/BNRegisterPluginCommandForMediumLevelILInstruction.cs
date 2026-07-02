using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNRegisterPluginCommandForMediumLevelILInstruction(const char* name, const char* description, void** action, void** isValid, void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterPluginCommandForMediumLevelILInstruction"
        )]
		internal static extern void BNRegisterPluginCommandForMediumLevelILInstruction(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* description
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string description  , 
			
			// void** action
		    IntPtr action  , 
			
			// void** isValid
		    IntPtr isValid  , 
			
			// void* context
		    IntPtr context  
			
		);
	}
}