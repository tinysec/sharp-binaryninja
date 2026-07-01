using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNInstructionInfo
	{
		/// <summary>
		/// 
		/// uint64_t length
		/// </summary>
		internal ulong length;
		
		/// <summary>
		/// 
		/// uint64_t branchCount
		/// </summary>
		internal ulong branchCount;
		
		/// <summary>
		/// 
		/// bool archTransitionByTargetAddr
		/// </summary>
		internal bool archTransitionByTargetAddr;
		
		/// <summary>
		/// 
		/// uint8_t delaySlots
		/// </summary>
		internal byte delaySlots;
		
		/// <summary>
		/// 
		/// BNBranchType[3] branchType
		/// </summary>
		internal fixed uint branchType[3];
		
		/// <summary>
		/// 
		/// uint64_t[3] branchTarget
		/// </summary>
		internal fixed ulong branchTarget[3];

		/// <summary>
		/// 
		/// BNArchitecture*[3] branchArch
		/// </summary>
		internal IntPtr branchArch_0;
		internal IntPtr branchArch_1;
		internal IntPtr branchArch_2;
	}
	
    public sealed class InstructionInfo 
    {	
		public ulong Length { get;} = 0;
		
		public ulong BranchCount { get;} = 0;

		public bool ArchTransitionByTargetAddr { get;} = false;
	
		public byte DelaySlots { get; } = 0;
		
		public BranchType[] BranchType { get;} = Array.Empty<BranchType>();
		
		public ulong[] BranchTarget { get;} = Array.Empty<ulong>();
		
		public Architecture[] BranchArch { get;} = Array.Empty<Architecture>();
		
		public InstructionInfo() 
		{
		    
		}
		
		public InstructionInfo(BNInstructionInfo native)
		{
			this.Length = native.length;

			this.BranchCount = native.branchCount;

			this.ArchTransitionByTargetAddr = native.archTransitionByTargetAddr;

			this.DelaySlots = native.delaySlots;
			
			// BranchType
			List<BranchType> branchTypes = new List<BranchType>();
			
			for (ulong i = 0; i < this.BranchCount; i++)
			{
				unsafe
				{
					branchTypes.Add( (BranchType)native.branchType[i] );
				}
			}

			this.BranchType = branchTypes.ToArray();
			
			// BranchTarget
			List<ulong> branchTargets = new List<ulong>();
			
			for (ulong i = 0; i < this.BranchCount; i++)
			{
				unsafe
				{
					branchTargets.Add( native.branchTarget[i] );
				}
			}

			this.BranchTarget = branchTargets.ToArray();
			
			// BranchArch
			List<Architecture> branchArches = new List<Architecture>();

			if (this.BranchCount >= 1)
			{
				if (IntPtr.Zero != native.branchArch_0)
				{
					branchArches.Add( new Architecture( native.branchArch_0 ) );
				}
			}

			if (this.BranchCount >= 2)
			{
				if (IntPtr.Zero != native.branchArch_1)
				{
					branchArches.Add( new Architecture( native.branchArch_1 ) );
				}
			}

			if (this.BranchCount >= 3)
			{
				if (IntPtr.Zero != native.branchArch_2)
				{
					branchArches.Add( new Architecture( native.branchArch_2 ) );
				}
			}
			
			this.BranchArch = branchArches.ToArray();
		}

		internal static InstructionInfo FromNative(BNInstructionInfo native)
		{
			return new InstructionInfo(native);
		}
    }
}