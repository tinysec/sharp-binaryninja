using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNBasicBlock** BNPrepareToCopyForeignFunction(BNLowLevelILFunction* dst, BNLowLevelILFunction* src, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPrepareToCopyForeignFunction"
        )]
		internal static extern IntPtr BNPrepareToCopyForeignFunction(
			
			// BNLowLevelILFunction* dst
		    IntPtr dst   , 
			
			// BNLowLevelILFunction* src
		    IntPtr src   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
