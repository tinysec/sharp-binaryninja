using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDecode(BNTransform* xform, BNDataBuffer* input, BNDataBuffer* output, BNTransformParameter* @params, uint64_t paramCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNDecode"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNDecode(
			
			// BNTransform* xform
		    IntPtr xform  , 
			
			// BNDataBuffer* input
		    IntPtr input  , 
			
			// BNDataBuffer* output
		    IntPtr output  , 
			
			// BNTransformParameter* _params
			BNTransformParameter[] _params  , 
			
			// uint64_t paramCount
		    ulong paramCount  
		);
	}
}