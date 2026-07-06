using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNConnectEnterpriseServer()
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNConnectEnterpriseServer"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNConnectEnterpriseServer();
	}
}