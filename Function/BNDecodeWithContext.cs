using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDecodeWithContext(BNTransform* xform, BNTransformContext* context, BNTransformParameter* @params, uint64_t paramCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNDecodeWithContext"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNDecodeWithContext(
			
			// BNTransform* xform
		    IntPtr xform  , 
			
			// BNTransformContext* context
		    IntPtr context  , 
			
			// BNTransformParameter* _params
			BNTransformParameter[] _params  , 
			
			// uint64_t paramCount
		    ulong paramCount  
		);
	}
}