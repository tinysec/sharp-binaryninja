using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public sealed class LowLevelILBasicBlockEdge : AbstractBasicBlockEdge<LowLevelILBasicBlockEdge,LowLevelILBasicBlock>
	{
		internal LowLevelILBasicBlockEdge( 
			BNBasicBlockEdge native ,
			LowLevelILBasicBlock source,
			LowLevelILBasicBlock target,
			bool outgoing
		) : base(native, source , target , outgoing)
		{
		    
		}

		internal static LowLevelILBasicBlockEdge FromNativeEx(
			BNBasicBlockEdge native ,
			LowLevelILBasicBlock me,
			bool outgoing
		)
		{
			if (outgoing)
			{
				return new LowLevelILBasicBlockEdge(
					native, 
					me , 
					LowLevelILBasicBlock.MustNewFromHandleEx(me.ILFunction, native.target),
					outgoing
				);
			}
			else
			{
				return new LowLevelILBasicBlockEdge(
					native, 
					LowLevelILBasicBlock.MustNewFromHandleEx(me.ILFunction,native.target) , 
					me,
					outgoing
				);
			}
		}
	}
}
