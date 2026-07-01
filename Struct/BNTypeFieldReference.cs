using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNTypeFieldReference 
	{
		/// <summary>
		/// BNFunction* func
		/// </summary>
		public IntPtr func;
		
		/// <summary>
		/// BNArchitecture* arch
		/// </summary>
		public IntPtr arch;
		
		/// <summary>
		/// uint64_t addr
		/// </summary>
		public ulong addr;
		
		/// <summary>
		/// uint64_t size
		/// </summary>
		public ulong size;
		
		/// <summary>
		/// BNTypeWithConfidence incomingType
		/// </summary>
		public BNTypeWithConfidence incomingType;
	}

    public class TypeFieldReference
    {
	    public Function? Function { get; set; } = null;
	    
	    public Architecture? Architecture { get; set; } = null;
	    
		public ulong Address { get; set; } = 0;
		
		public ulong Size { get; set; } = 0;

		public TypeWithConfidence IncomingType { get; set; }
		
		internal TypeFieldReference(BNTypeFieldReference native)
		{
		    this.Function = Function.NewFromHandle(native.func);
		    this.Architecture = Architecture.FromHandle(native.arch);
		    this.Address = native.addr;
		    this.Size = native.size;
		    this.IncomingType = TypeWithConfidence.FromNative(native.incomingType);
		}

		internal static TypeFieldReference FromNative(BNTypeFieldReference native)
		{
		    return new TypeFieldReference(native);
		}
    }
}