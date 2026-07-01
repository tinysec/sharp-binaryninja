using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNIsConstantRendererValidForType( BNConstantRenderer* renderer, BNHighLevelILFunction* il, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsConstantRendererValidForType"
        )]
		internal static extern bool BNIsConstantRendererValidForType(
			
			// BNConstantRenderer* renderer
		    IntPtr renderer   , 
			
			// BNHighLevelILFunction* il
		    IntPtr il   , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}
