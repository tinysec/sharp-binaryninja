using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static void AddRequiredPluginDependency(string name)
		{
			NativeMethods.BNAddRequiredPluginDependency(name);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddRequiredPluginDependency(const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddRequiredPluginDependency"
        )]
		public static extern void BNAddRequiredPluginDependency(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
		);
	}
}