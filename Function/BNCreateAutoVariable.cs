using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNCreateAutoVariable(BNFunction* func, BNVariable* var, BNTypeWithConfidence* type, const char* name, bool ignoreDisjointUses)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateAutoVariable"
        )]
		internal static extern void BNCreateAutoVariable(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNVariable* _var
		    IntPtr _var  , 
			
			// BNTypeWithConfidence* type
		    IntPtr type  , 
			
			// const char* name
		    string name  , 
			
			// bool ignoreDisjointUses
		    bool ignoreDisjointUses  
		);
	}
}