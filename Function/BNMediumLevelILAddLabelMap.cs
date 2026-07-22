using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// uint64_t BNMediumLevelILAddLabelMap(BNMediumLevelILFunction* func, uint64_t* values, BNMediumLevelILLabel** labels, uint64_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNMediumLevelILAddLabelMap"
        )]
		internal static extern MediumLevelILExpressionIndex BNMediumLevelILAddLabelMap(
			
			// BNMediumLevelILFunction* func
		    IntPtr func  , 
			
			// uint64_t* values
		    ulong[] values  , 
			
			// BNMediumLevelILLabel** labels
			IntPtr[] labels  ,
			
			// uint64_t count
		    ulong count  
		);
	}
}
