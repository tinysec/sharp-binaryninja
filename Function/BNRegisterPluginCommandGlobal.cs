using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNRegisterPluginCommandGlobal(const char* name, const char* description, void (*action)(void* ctxt), bool (*isValid)(void* ctxt), void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore",
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterPluginCommandGlobal"
        )]
		internal static extern void BNRegisterPluginCommandGlobal(

			// const char* name
		    string name  ,

			// const char* description
		    string description  ,

			// void (*action)(void* ctxt)
		    IntPtr action  ,

			// bool (*isValid)(void* ctxt)
		    IntPtr isValid  ,

			// void* context
		    IntPtr context
		);
	}
}
