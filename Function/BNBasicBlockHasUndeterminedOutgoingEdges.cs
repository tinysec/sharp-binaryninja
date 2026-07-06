using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBasicBlockHasUndeterminedOutgoingEdges(BNBasicBlock* block)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBasicBlockHasUndeterminedOutgoingEdges"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNBasicBlockHasUndeterminedOutgoingEdges(
			
			// BNBasicBlock* block
		    IntPtr block  
		);
	}
}