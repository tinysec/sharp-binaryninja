using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetCallingConventionDefaultRegisterStackAdjustments(BNCallingConvention* cc, BNValueLocation* returnValue, BNValueLocation* params, size_t paramCount, uint32_t** outRegs, int32_t** outAdjust)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetCallingConventionDefaultRegisterStackAdjustments"
        )]
		internal static extern UIntPtr BNGetCallingConventionDefaultRegisterStackAdjustments(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNValueLocation* returnValue
		    IntPtr returnValue   , 
			
			// BNValueLocation* params
		    IntPtr @params   , 
			
			// size_t paramCount
		    UIntPtr paramCount   , 
			
			// uint32_t** outRegs
		    IntPtr outRegs   , 
			
			// int32_t** outAdjust
		    IntPtr outAdjust  
		);
	}
}
