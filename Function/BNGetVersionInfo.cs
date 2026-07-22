using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static VersionInfo GetVersionInfo()
		{
			return VersionInfo.TakeNative(
				NativeMethods.BNGetVersionInfo()
			);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
	    /// BNVersionInfo BNGetVersionInfo()
	    /// </summary>
	    [DllImport(
		    "binaryninjacore", 
		    CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
		    EntryPoint = "BNGetVersionInfo"
	    )]
	    internal static extern BNVersionInfo BNGetVersionInfo();
    }
}
