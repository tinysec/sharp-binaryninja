using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNValueLocation* BNGetParameterLocations(BNCallingConvention* cc, BNBinaryView* view, BNValueLocation* returnValue, BNFunctionParameter* params, size_t paramCount, const uint32_t* permittedRegs, size_t permittedRegCount, size_t* outCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetParameterLocations"
        )]
		internal static extern IntPtr BNGetParameterLocations(
			
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
			
			// const uint32_t* permittedRegs
		    IntPtr permittedRegs   , 
			
			// size_t permittedRegCount
		    UIntPtr permittedRegCount   , 
			
			// size_t* outCount
		    IntPtr outCount  
		);
	}
}
