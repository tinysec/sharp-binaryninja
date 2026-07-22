using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	internal static partial class NativeDelegates
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNDemanglerIsMangledString(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string name
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		internal delegate bool BNDemanglerDemangle(
			IntPtr context,
			IntPtr architecture,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string name,
			IntPtr outType,
			IntPtr outVarName,
			IntPtr view
		);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		internal delegate void BNDemanglerFreeVarName(
			IntPtr context,
			IntPtr name
		);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BNDemanglerCallbacks
	{
		/// <summary>
		/// void* context
		/// </summary>
		public IntPtr context;
		
		/// <summary>
		/// void** isMangledString
		/// </summary>
		public IntPtr isMangledString;
		
		/// <summary>
		/// void** demangle
		/// </summary>
		public IntPtr demangle;
		
		/// <summary>
		/// void** freeVarName
		/// </summary>
		public IntPtr freeVarName;
	}

	public class DemanglerCallbacks
    {
		public DemanglerCallbacks() 
		{
		    
		}
    }
}
