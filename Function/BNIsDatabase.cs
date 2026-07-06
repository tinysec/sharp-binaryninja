using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
	{
		public static bool IsDatabase(string filename)
		{
			return NativeMethods.BNIsDatabase(filename);
		}
	}
	
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsDatabase(const char* filename)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNIsDatabase(
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename  
		);
	}
}