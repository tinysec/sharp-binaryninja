using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	public static partial class Core
    {
	    public static string GenerateAutoTypeId(string source  , QualifiedName name)
	    {
		    using (ScopedAllocator allocator = new ScopedAllocator())
		    {
			    return UnsafeUtils.TakeAnsiString(

				    NativeMethods.BNGenerateAutoTypeId(
					    source ,
					    name.ToNativeEx(allocator)
				    )
			    );
		    }
	    }
	}
    
    internal static partial class NativeMethods
    {
	    /// <summary>
	    /// char* BNGenerateAutoTypeId(const char* source, BNQualifiedName* name)
	    /// </summary>
	    [DllImport(
		    "binaryninjacore", 
		    CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
		    EntryPoint = "BNGenerateAutoTypeId"
	    )]
	    internal static extern IntPtr BNGenerateAutoTypeId(
			
		    // const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  , 
			
		    // BNQualifiedName* name
		    in BNQualifiedName name  
	    );
    }
}