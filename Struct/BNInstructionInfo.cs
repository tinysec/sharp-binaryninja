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
		[MarshalAs(UnmanagedType.I1)] internal bool archTransitionByTargetAddr;
		
		/// <summary>
		/// 
		/// uint8_t delaySlots
		/// </summary>
		internal byte delaySlots;
		
		/// <summary>
		///
		/// BNBranchType[3] branchType
		/// </summary>
		// BNBranchType is a 1-byte enum (BN_ENUM(uint8_t, ...)); the fixed buffer element
		// MUST be 1 byte. A 4-byte element would size branchType at 12 bytes instead of 3
		// and shift branchTarget/branchArch, so the core's out-param fields are read at the
		// wrong offsets (garbage branch targets). BranchType is `enum : byte`, so the wrapper
		// cast of each byte to BranchType is exact.
		internal fixed byte branchType[3];
		
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
		private const int MaximumBranchCount = 3;

		public ulong Length { get; set; } = 0;

		public ulong BranchCount { get; private set; } = 0;

		public bool ArchTransitionByTargetAddr { get; set; } = false;

		public byte DelaySlots { get; set; } = 0;

		public BranchType[] BranchType { get; private set; } = Array.Empty<BranchType>();

		public ulong[] BranchTarget { get; private set; } = Array.Empty<ulong>();

		public Architecture?[] BranchArch { get; private set; } = Array.Empty<Architecture?>();
		
		public InstructionInfo() 
		{
		    
		}
		
		public InstructionInfo(BNInstructionInfo native)
		{
			this.Length = native.length;

			this.BranchCount = Math.Min(native.branchCount, (ulong)MaximumBranchCount);

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
			List<Architecture?> branchArches = new List<Architecture?>();

			if (1 <= this.BranchCount)
			{
				branchArches.Add(Architecture.FromHandle(native.branchArch_0));
			}

			if (2 <= this.BranchCount)
			{
				branchArches.Add(Architecture.FromHandle(native.branchArch_1));
			}

			if (3 <= this.BranchCount)
			{
				branchArches.Add(Architecture.FromHandle(native.branchArch_2));
			}
			
			this.BranchArch = branchArches.ToArray();
		}

		public void AddBranch(
			BranchType type,
			ulong target = 0,
			Architecture? architecture = null,
			byte delaySlots = 0)
		{
			if (MaximumBranchCount <= this.BranchCount)
			{
				return;
			}

			List<BranchType> branchTypes = new List<BranchType>(this.BranchType);
			List<ulong> branchTargets = new List<ulong>(this.BranchTarget);
			List<Architecture?> branchArchitectures =
				new List<Architecture?>(this.BranchArch);

			branchTypes.Add(type);
			branchTargets.Add(target);
			branchArchitectures.Add(architecture);

			this.BranchType = branchTypes.ToArray();
			this.BranchTarget = branchTargets.ToArray();
			this.BranchArch = branchArchitectures.ToArray();
			this.BranchCount = (ulong)this.BranchType.Length;
			this.DelaySlots = delaySlots;
		}

		internal unsafe BNInstructionInfo ToNative()
		{
			BNInstructionInfo native = new BNInstructionInfo
			{
				length = this.Length,
				branchCount = this.BranchCount,
				archTransitionByTargetAddr = this.ArchTransitionByTargetAddr,
				delaySlots = this.DelaySlots
			};

			for (int index = 0; index < this.BranchType.Length; index++)
			{
				native.branchType[index] = (byte)this.BranchType[index];
				native.branchTarget[index] = this.BranchTarget[index];
			}

			native.branchArch_0 = this.GetBranchArchitectureHandle(0);
			native.branchArch_1 = this.GetBranchArchitectureHandle(1);
			native.branchArch_2 = this.GetBranchArchitectureHandle(2);

			return native;
		}

		private IntPtr GetBranchArchitectureHandle(int index)
		{
			if (this.BranchArch.Length <= index || null == this.BranchArch[index])
			{
				return IntPtr.Zero;
			}

			return this.BranchArch[index]!.DangerousGetHandle();
		}

		internal static InstructionInfo FromNative(BNInstructionInfo native)
		{
			return new InstructionInfo(native);
		}
    }
}
