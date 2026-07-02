using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeDefinitionLine* BNGetTypeLines(BNType* type, BNTypeContainer* types, const char* name, int32_t paddingCols, bool collapsed, BNTokenEscapingType escaping, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeLines"
        )]
		internal static extern IntPtr BNGetTypeLines(
			
			// BNType* type
		    IntPtr type  , 
			
			// BNTypeContainer* types
		    IntPtr types  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// int32_t paddingCols
		    int paddingCols  , 
			
			// bool collapsed
		    bool collapsed  , 
			
			// BNTokenEscapingType escaping
		    TokenEscapingType escaping  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}