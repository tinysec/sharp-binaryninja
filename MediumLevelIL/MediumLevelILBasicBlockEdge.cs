using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public sealed class MediumLevelILBasicBlockEdge : AbstractBasicBlockEdge<MediumLevelILBasicBlockEdge,MediumLevelILBasicBlock>
    {
	    internal MediumLevelILBasicBlockEdge( 
		    BNBasicBlockEdge native ,
		    MediumLevelILBasicBlock source,
		    MediumLevelILBasicBlock target,
		    bool outgoing
		) : base(native, source , target , outgoing)
	    {
		    
	    }

	    internal static MediumLevelILBasicBlockEdge FromNativeEx(
		    BNBasicBlockEdge native ,
		    MediumLevelILBasicBlock me,
		    bool outgoing
	    )
	    {
		    if (outgoing)
		    {
			    return new MediumLevelILBasicBlockEdge(
				    native, 
				    me , 
				    MediumLevelILBasicBlock.MustNewFromHandleEx(me.ILFunction, native.target),
				    outgoing
			    );
		    }
		    else
		    {
			    return new MediumLevelILBasicBlockEdge(
				    native, 
				    MediumLevelILBasicBlock.MustNewFromHandleEx(me.ILFunction,native.target) , 
				    me,
				    outgoing
			    );
		    }
	    }
    }
}
