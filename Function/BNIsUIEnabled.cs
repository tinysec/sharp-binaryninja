using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	
	public static partial class Core
	{
		public static bool IsUIEnabled()
		{
			return NativeMethods.BNIsUIEnabled();
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsUIEnabled()
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsUIEnabled"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNIsUIEnabled();
	}
}