using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static bool IsLicenseValidated()
		{
			return NativeMethods.BNIsLicenseValidated();
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsLicenseValidated()
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsLicenseValidated"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNIsLicenseValidated();
	}
}