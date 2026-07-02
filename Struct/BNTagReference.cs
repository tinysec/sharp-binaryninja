using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNTagReference 
	{
		/// <summary>
		/// BNTagReferenceType refType
		/// </summary>
		internal TagReferenceType refType;
		
		/// <summary>
		/// bool autoDefined
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool autoDefined;
		
		/// <summary>
		/// BNTag* tag
		/// </summary>
		internal IntPtr tag;
		
		/// <summary>
		/// BNArchitecture* arch
		/// </summary>
		internal IntPtr arch;
		
		/// <summary>
		/// BNFunction* func
		/// </summary>
		internal IntPtr function;
		
		/// <summary>
		/// uint64_t addr
		/// </summary>
		internal ulong address;
	}

    public class TagReference : INativeWrapper<BNTagReference>
    {
	    public TagReferenceType RefType { get; set; } = TagReferenceType.AddressTagReference;

		public bool AutoDefined { get; set; } = false;

		public Tag Tag { get; set; }

		public Architecture? Architecture { get; set; } = null;
		
		public Function? Function { get; set; } = null;
	
		public ulong Address { get; set; } = 0;
		
		public TagReference(Tag tag) 
		{
		    this.Tag = tag;
		}

		internal static TagReference FromNative(BNTagReference native)
		{
			return new TagReference(Tag.MustNewFromHandle(native.tag))
			{
				RefType = native.refType ,
				AutoDefined = native.autoDefined ,
				Architecture = Architecture.FromHandle(native.arch) ,
				Function = Function.NewFromHandle(native.function) ,
			};
		}

		public BNTagReference ToNative()
		{
			return new BNTagReference
			{
				refType = this.RefType ,
				autoDefined = this.AutoDefined ,
				tag = ( null == this.Tag ? IntPtr.Zero : this.Tag.DangerousGetHandle() ) ,
				arch = ( null == this.Architecture ? IntPtr.Zero : this.Architecture.DangerousGetHandle() ) ,
				function = ( null == this.Function ? IntPtr.Zero : this.Function.DangerousGetHandle() )
			};
		}
    }
}