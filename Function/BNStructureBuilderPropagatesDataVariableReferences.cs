using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNStructureBuilderPropagatesDataVariableReferences(BNStructureBuilder* s)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNStructureBuilderPropagatesDataVariableReferences"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNStructureBuilderPropagatesDataVariableReferences(
			
			// BNStructureBuilder* s
		    IntPtr s  
		);
	}
}