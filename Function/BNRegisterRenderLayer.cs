using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRenderLayer* BNRegisterRenderLayer(const char* name, BNRenderLayerCallbacks* callbacks, BNRenderLayerDefaultEnableState enableState)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRegisterRenderLayer"
        )]
		internal static extern IntPtr BNRegisterRenderLayer(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNRenderLayerCallbacks* callbacks
		    in BNRenderLayerCallbacks callbacks  ,
			
			// BNRenderLayerDefaultEnableState enableState
		    RenderLayerDefaultEnableState enableState  
		);
	}
}
