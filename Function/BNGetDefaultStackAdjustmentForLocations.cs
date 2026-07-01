using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// int64_t BNGetDefaultStackAdjustmentForLocations(BNCallingConvention* cc, BNValueLocation* returnValue, const BNValueLocation* paramLocations, const BNType** paramTypes, size_t paramCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDefaultStackAdjustmentForLocations"
        )]
		internal static extern long BNGetDefaultStackAdjustmentForLocations(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNValueLocation* returnValue
		    IntPtr returnValue   , 
			
			// const BNValueLocation* paramLocations
		    IntPtr paramLocations   , 
			
			// const BNType** paramTypes
		    IntPtr paramTypes   , 
			
			// size_t paramCount
		    UIntPtr paramCount  
		);
	}
}
