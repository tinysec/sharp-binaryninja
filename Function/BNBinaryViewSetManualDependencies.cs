using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNBinaryViewSetManualDependencies(BNBinaryView* view, BNQualifiedName* viewTypeNames, BNQualifiedName* libTypeNames, const char** libNames, uint64_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNBinaryViewSetManualDependencies"
        )]
		internal static extern void BNBinaryViewSetManualDependencies(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNQualifiedName* viewTypeNames
		    BNQualifiedName[] viewTypeNames  , 
			
			// BNQualifiedName* libTypeNames
			BNQualifiedName[] libTypeNames  , 
			
			// const char** libNames
		    IntPtr libNames  , 
			
			// uint64_t count
		    ulong count  
		);
	}
}