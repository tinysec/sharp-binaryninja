using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static void SetLicense(string licenseData  )
		{
			NativeMethods.BNSetLicense(licenseData);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetLicense(const char* licenseData)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetLicense"
        )]
		public static extern void BNSetLicense(
			
			// const char* licenseData
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string licenseData  
		);
	}
}