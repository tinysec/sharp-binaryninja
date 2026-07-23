using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public delegate bool ProgressDelegate(
		
		// uint64_t param2
		ulong param2 , 
		
		// uint64_t param3
		ulong param3 
	);
	
	internal static partial class NativeDelegates
	{
		/// <summary>
		///  
		/// typedef bool (*BNProgressFunction)(void* param1, uint64_t param2, uint64_t param3)
		/// </summary>
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNProgressFunction(
		
			// void* param1
			IntPtr param1 , 
		
			// uint64_t param2
			ulong param2 , 
		
			// uint64_t param3
			ulong param3 
		);
	}

	internal static partial class UnsafeUtils
	{
		internal static NativeDelegates.BNProgressFunction WrapProgressDelegate(
			ProgressDelegate callback)
		{
			return bool (param1 , param2 , param3) =>
			{
				return callback(param2 , param3);
			};
		}
	}
}
