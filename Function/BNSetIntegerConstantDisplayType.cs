using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetIntegerConstantDisplayType(BNFunction* func, BNArchitecture* arch, uint64_t instrAddr, uint64_t value, uint64_t operand, BNIntegerDisplayType type, const char* typeID)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetIntegerConstantDisplayType"
        )]
		internal static extern void BNSetIntegerConstantDisplayType(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t instrAddr
		    ulong instrAddr  , 
			
			// uint64_t _value
		    ulong _value  , 
			
			// uint64_t operand
		    ulong operand  , 
			
			// BNIntegerDisplayType type
		    IntegerDisplayType type  , 
			
			// const char* typeID
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeID  
			
		);
	}
}