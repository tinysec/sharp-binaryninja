using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNCallLayout BNGetDefaultCallLayout(BNCallingConvention* cc, BNBinaryView* view, const BNReturnValue* returnValue, const BNFunctionParameter* params, size_t paramCount, const uint32_t* permittedRegs, size_t permittedRegCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDefaultCallLayout"
        )]
		internal static extern BNCallLayout BNGetDefaultCallLayout(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// const BNReturnValue* returnValue
		    IntPtr returnValue   , 
			
			// const BNFunctionParameter* params
		    IntPtr @params   , 
			
			// size_t paramCount
		    UIntPtr paramCount   , 
			
			// const uint32_t* permittedRegs
		    IntPtr permittedRegs   , 
			
			// size_t permittedRegCount
		    UIntPtr permittedRegCount  
		);
	}
}
