using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static bool InitPlugins(bool allowUserPlugins )
		{
			return NativeMethods.BNInitPlugins(allowUserPlugins);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNInitPlugins(bool allowUserPlugins)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNInitPlugins"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNInitPlugins(
			
			// bool allowUserPlugins
		    bool allowUserPlugins  
		);
	}
}