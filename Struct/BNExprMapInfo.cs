using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNExprMapInfo 
	{
		/// <summary>
		/// uint64_t lowerIndex
		/// </summary>
		public ulong lowerIndex;
		
		/// <summary>
		/// uint64_t higherIndex
		/// </summary>
		public ulong higherIndex;
		
		/// <summary>
		/// bool mapLowerToHigher
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool mapLowerToHigher;
		
		/// <summary>
		/// bool mapHigherToLower
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool mapHigherToLower;
		
		/// <summary>
		/// bool lowerToHigherDirect
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool lowerToHigherDirect;
		
		/// <summary>
		/// bool higherToLowerDirect
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool higherToLowerDirect;
	}

    public class ExprMapInfo 
    {
		public ulong LowerIndex { get; set; } = 0;
		
		public ulong HigherIndex { get; set; } = 0;
		
		public bool MapLowerToHigher { get; set; } = false;
		
		public bool MapHigherToLower { get; set; } = false;
		
		public bool LowerToHigherDirect { get; set; } = false;
		
		public bool HigherToLowerDirect { get; set; } = false;
		
		public ExprMapInfo() 
		{
		    
		}

		public ExprMapInfo(
			ulong lowerIndex,
			ulong higherIndex,
			bool mapLowerToHigher,
			bool mapHigherToLower,
			bool lowerToHigherDirect,
			bool higherToLowerDirect)
		{
			this.LowerIndex = lowerIndex;
			this.HigherIndex = higherIndex;
			this.MapLowerToHigher = mapLowerToHigher;
			this.MapHigherToLower = mapHigherToLower;
			this.LowerToHigherDirect = lowerToHigherDirect;
			this.HigherToLowerDirect = higherToLowerDirect;
		}

		internal BNExprMapInfo ToNative()
		{
			return new BNExprMapInfo
			{
				lowerIndex = this.LowerIndex,
				higherIndex = this.HigherIndex,
				mapLowerToHigher = this.MapLowerToHigher,
				mapHigherToLower = this.MapHigherToLower,
				lowerToHigherDirect = this.LowerToHigherDirect,
				higherToLowerDirect = this.HigherToLowerDirect,
			};
		}
    }

	/// <summary>
	/// Describes one LLIL SSA expression to MLIL expression mapping.
	/// </summary>
	public sealed class LLILSSAToMLILExpressionMap : ExprMapInfo
	{
		public LLILSSAToMLILExpressionMap(
			LowLevelILExpressionIndex lowerIndex,
			MediumLevelILExpressionIndex higherIndex,
			bool mapLowerToHigher,
			bool mapHigherToLower,
			bool lowerToHigherDirect,
			bool higherToLowerDirect)
			: base(
				(ulong)lowerIndex,
				(ulong)higherIndex,
				mapLowerToHigher,
				mapHigherToLower,
				lowerToHigherDirect,
				higherToLowerDirect)
		{
		}
	}
}
