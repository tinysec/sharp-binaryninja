using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerAddTypes(BNTypeContainer* container, BNQualifiedName* typeNames, BNType** types, uint64_t typeCount, void* progress, void* progressContext, BNQualifiedName** resultNames,  char*** resultIds, uint64_t* resultCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeContainerAddTypes"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerAddTypes(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// BNQualifiedName* typeNames
			BNQualifiedName[] typeNames  , 
			
			// BNType** types
		    IntPtr types  , 
			
			// uint64_t typeCount
		    ulong typeCount  , 
			
			// void* progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  , 
			
			// BNQualifiedName** resultNames
		    out IntPtr resultNames  , 
			
			// char*** resultIds
			out IntPtr resultIds  , 
			
			// uint64_t* resultCount
		    out ulong resultCount  
		);
	}
}