using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static void AddOptionalPluginDependency(string name)
		{
			NativeMethods.BNAddOptionalPluginDependency(name);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddOptionalPluginDependency(const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddOptionalPluginDependency"
        )]
		public static extern void BNAddOptionalPluginDependency(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
		);
	}
}