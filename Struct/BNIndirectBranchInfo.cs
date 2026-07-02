using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNIndirectBranchInfo 
	{
		/// <summary>
		/// BNArchitecture* sourceArch
		/// </summary>
		public IntPtr sourceArch;
		
		/// <summary>
		/// uint64_t sourceAddr
		/// </summary>
		public ulong sourceAddr;
		
		/// <summary>
		/// BNArchitecture* destArch
		/// </summary>
		public IntPtr destArch;
		
		/// <summary>
		/// uint64_t destAddr
		/// </summary>
		public ulong destAddr;
		
		/// <summary>
		/// bool autoDefined
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool autoDefined;
	}

    public sealed class IndirectBranchInfo 
    {
		public Architecture SourceArchitecture { get; set; }
		
		public ulong SourceAddress { get; set; } = 0;
		
		public Architecture DestArchitecture { get; set; }
		
		public ulong DestAddress { get; set; } = 0;
		
		public bool AutoDefined { get; set; } = false;
		
		public IndirectBranchInfo(
			Architecture sourceArch,
			ulong sourceAddress,
			Architecture destArch,
			ulong destAddress,
			bool autoDefined
		) 
		{
		    this.SourceArchitecture = sourceArch;
		    this.SourceAddress = sourceAddress;
		    this.DestArchitecture = destArch;
		    this.DestAddress = destAddress;
		    this.AutoDefined = autoDefined;
		}

		internal static IndirectBranchInfo FromNative(BNIndirectBranchInfo native)
		{
			return new IndirectBranchInfo(
				Architecture.MustFromHandle(native.sourceArch) ,
				native.sourceAddr ,
				Architecture.MustFromHandle(native.destArch) ,
				native.destAddr ,
				native.autoDefined
			);
		}
    }
}