using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNFlagConditionForSemanticClass 
	{
		/// <summary>
		/// uint32_t semanticClass
		/// </summary>
		internal uint semanticClass;
		
		/// <summary>
		/// BNLowLevelILFlagCondition condition
		/// </summary>
		internal LowLevelILFlagCondition condition;
	}

    public sealed class FlagConditionForSemanticClass : INativeWrapper<BNFlagConditionForSemanticClass>
    {
		public uint SemanticClass { get; } = 0;
		
		public LowLevelILFlagCondition Condition { get;  } = LowLevelILFlagCondition.LLFC_E;

		public FlagConditionForSemanticClass(
			uint semanticClass,
			LowLevelILFlagCondition condition)
		{
			this.SemanticClass = semanticClass;
			this.Condition = condition;
		}
		
		public FlagConditionForSemanticClass(BNFlagConditionForSemanticClass native)
		{
			this.SemanticClass = native.semanticClass;
			this.Condition = native.condition;
		}

		internal static FlagConditionForSemanticClass FromNative(BNFlagConditionForSemanticClass native)
		{
			return new FlagConditionForSemanticClass(native);
		}

		public BNFlagConditionForSemanticClass ToNative()
		{
			return new BNFlagConditionForSemanticClass()
			{
				semanticClass = this.SemanticClass , 
				condition = this.Condition
			};
		}
    }
}
