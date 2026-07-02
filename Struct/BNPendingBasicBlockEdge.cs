using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNPendingBasicBlockEdge 
	{
		/// <summary>
		/// BNBranchType type
		/// </summary>
		public BranchType type;
		
		/// <summary>
		/// BNArchitecture* arch
		/// </summary>
		public IntPtr arch;
		
		/// <summary>
		/// uint64_t target
		/// </summary>
		public ulong target;
		
		/// <summary>
		/// bool fallThrough
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool fallThrough;
	}

    public sealed class PendingBasicBlockEdge 
    {
		public BranchType Type { get; set; } = BranchType.UnconditionalBranch;
		
		public Architecture? Architecture { get; set; } = null;
		
		public ulong Target { get; set; } = 0;
		
		public bool FallThrough { get; set; } = false;
		
		internal PendingBasicBlockEdge(BNPendingBasicBlockEdge native) 
		{
		    this.Type = native.type;

		    if (IntPtr.Zero != native.arch)
		    {
			    this.Architecture = new Architecture(native.arch);
		    }
		  
		    this.Target = native.target;
		    
		    this.FallThrough = native.fallThrough;
		}
		
		internal static PendingBasicBlockEdge FromNative(BNPendingBasicBlockEdge native) 
		{
			return new PendingBasicBlockEdge(native);
		}
    }
}