using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNValueLocation* BNGetDefaultParameterLocationsDefaultPermittedArgs(BNCallingConvention* cc, BNBinaryView* view, BNValueLocation* returnValue, BNFunctionParameter* params, size_t paramCount, size_t* outCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDefaultParameterLocationsDefaultPermittedArgs"
        )]
		internal static extern IntPtr BNGetDefaultParameterLocationsDefaultPermittedArgs(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// BNValueLocation* returnValue
		    IntPtr returnValue   , 
			
			// BNFunctionParameter* params
		    IntPtr @params   , 
			
			// size_t paramCount
		    UIntPtr paramCount   , 
			
			// size_t* outCount
		    IntPtr outCount  
		);
	}
}
