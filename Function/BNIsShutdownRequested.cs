using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static bool IsShutdownRequested()
		{
			return NativeMethods.BNIsShutdownRequested();
		}
	}
	
	internal static partial class NativeMethods
	{
		/// <summary>
		/// bool BNIsShutdownRequested()
		/// </summary>
		[DllImport(
			"binaryninjacore", 
			CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
			EntryPoint = "BNIsShutdownRequested"
		)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNIsShutdownRequested();
	}
}