using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNCallLayout BNGetDefaultCallLayoutDefaultPermittedArgs(BNCallingConvention* cc, BNBinaryView* view, const BNReturnValue* returnValue, const BNFunctionParameter* params, size_t paramCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDefaultCallLayoutDefaultPermittedArgs"
        )]
		internal static extern BNCallLayout BNGetDefaultCallLayoutDefaultPermittedArgs(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// const BNReturnValue* returnValue
		    IntPtr returnValue   , 
			
			// const BNFunctionParameter* params
		    IntPtr @params   , 
			
			// size_t paramCount
		    UIntPtr paramCount  
		);
	}
}
