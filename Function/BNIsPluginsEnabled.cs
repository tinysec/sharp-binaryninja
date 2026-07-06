using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static bool IsPluginsEnabled()
		{
			return NativeMethods.BNIsPluginsEnabled();
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsPluginsEnabled()
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsPluginsEnabled"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNIsPluginsEnabled();
	}
}