using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNReferenceSource 
	{
		/// <summary>
		/// BNFunction* func
		/// </summary>
		internal IntPtr function;
		
		/// <summary>
		/// BNArchitecture* arch
		/// </summary>
		internal IntPtr arch;
		
		/// <summary>
		/// uint64_t addr
		/// </summary>
		internal ulong address;
	}

    public class ReferenceSource : INativeWrapper<BNReferenceSource>
    {
	    public Function? Function { get; set; } = null;
	    
		public Architecture? Architecture { get; set; } = null;
		
		public ulong Address { get; set; } = 0;
		
		public ReferenceSource()
		{

		}

		/// <summary>
		/// The Low Level IL instruction at this reference, or <c>null</c>.
		/// Mirrors Python <c>ReferenceSource.llil</c>.
		/// </summary>
		public LowLevelILInstruction? Llil
		{
			get
			{
				return this.Function?.GetLowLevelILAt(this.Address , this.Architecture);
			}
		}

		/// <summary>
		/// The Medium Level IL instruction at this reference, or <c>null</c>.
		/// Mirrors Python <c>ReferenceSource.mlil</c>.
		/// </summary>
		public MediumLevelILInstruction? Mlil
		{
			get
			{
				return this.Function?.GetMediumLevelILAt(this.Address , this.Architecture);
			}
		}

		/// <summary>
		/// The High Level IL instruction at this reference, or <c>null</c>.
		/// Mirrors Python <c>ReferenceSource.hlil</c>.
		/// </summary>
		public HighLevelILInstruction? Hlil
		{
			get
			{
				return this.Function?.GetHighLevelILAt(this.Address , this.Architecture);
			}
		}

		internal static ReferenceSource FromNative(BNReferenceSource raw)
		{
			return new ReferenceSource()
			{
				Function = Function.MustNewFromHandle(raw.function) ,
				Architecture = Architecture.MustFromHandle(raw.arch) ,
				Address = raw.address
			};
		}
		
		public BNReferenceSource ToNative()
		{
			return new BNReferenceSource()
			{
				function = ( null == this.Function ? IntPtr.Zero : this.Function.DangerousGetHandle() ),
				arch = (null == this.Architecture ? IntPtr.Zero :this.Architecture.DangerousGetHandle()) ,
				address = this.Address
			};
		}
    }
}