using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTransform* BNRegisterTransformTypeWithCapabilities(BNTransformType type, uint32_t capabilities, const char* name, const char* longName, const char* group, BNCustomTransform* xform)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterTransformTypeWithCapabilities"
        )]
		internal static extern IntPtr BNRegisterTransformTypeWithCapabilities(
			
			// BNTransformType type
		    TransformType type  , 
			
			// uint32_t capabilities
		    uint capabilities  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* longName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string longName  , 
			
			// const char* _group
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _group  , 
			
			// BNCustomTransform* xform
		    in BNCustomTransform xform  
		);
	}
}