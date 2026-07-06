using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNParsePossibleValueSet(BNBinaryView* view, const char* valueText, BNRegisterValueType state, BNPossibleValueSet* result, uint64_t here, const char** errors)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParsePossibleValueSet"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNParsePossibleValueSet(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// const char* valueText
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string valueText  , 
			
			// BNRegisterValueType state
		    RegisterValueType state  , 
			
			// BNPossibleValueSet* result
		    out BNPossibleValueSet result  , 
			
			// uint64_t here
		    ulong here  , 
			
			// const char** errors
		    out IntPtr errors  
		);
	}
}