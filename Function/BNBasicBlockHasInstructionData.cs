using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// const bool BNBasicBlockHasInstructionData(BNBasicBlock* block)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBasicBlockHasInstructionData"
        )]
		internal static extern bool BNBasicBlockHasInstructionData(
			
			// BNBasicBlock* block
		    IntPtr block  
		);
	}
}
