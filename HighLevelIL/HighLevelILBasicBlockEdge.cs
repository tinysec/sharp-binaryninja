using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public sealed class HighLevelILBasicBlockEdge : AbstractBasicBlockEdge<HighLevelILBasicBlockEdge,HighLevelILBasicBlock>
	{
		internal HighLevelILBasicBlockEdge( 
			BNBasicBlockEdge native ,
			HighLevelILBasicBlock source,
			HighLevelILBasicBlock target,
			bool outgoing
		) : base(native, source , target , outgoing)
		{
		    
		}

		internal static HighLevelILBasicBlockEdge FromNativeEx(
			BNBasicBlockEdge native ,
			HighLevelILBasicBlock me,
			bool outgoing
		)
		{
			if (outgoing)
			{
				return new HighLevelILBasicBlockEdge(
					native, 
					me , 
					HighLevelILBasicBlock.MustNewFromHandleEx(me.ILFunction, native.target),
					outgoing
				);
			}
			else
			{
				return new HighLevelILBasicBlockEdge(
					native, 
					HighLevelILBasicBlock.MustNewFromHandleEx(me.ILFunction,native.target) , 
					me,
					outgoing
				);
			}
		}
	}
}
