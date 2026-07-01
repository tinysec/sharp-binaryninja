using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNBasicBlockEdge 
	{
		/// <summary>
		/// BNBranchType type
		/// </summary>
		internal BranchType type;
		
		/// <summary>
		/// BNBasicBlock* target
		/// </summary>
		internal IntPtr target;
		
		/// <summary>
		/// bool backEdge
		/// </summary>
		internal bool backEdge;
		
		/// <summary>
		/// bool fallThrough
		/// </summary>
		internal bool fallThrough;
	}
	
	
	public abstract class AbstractBasicBlockEdge<T_SELF,T_BASICBLOCK> 
		 : IEquatable<T_SELF>, IComparable< T_SELF>
		where T_SELF : AbstractBasicBlockEdge<T_SELF,T_BASICBLOCK>
		where T_BASICBLOCK : AbstractBasicBlock<T_BASICBLOCK>
    {
	    public BranchType Type { get; } = BranchType.UnconditionalBranch;
		
	    public T_BASICBLOCK Source { get; }
		
	    public T_BASICBLOCK Target { get;  }
	    
	    public bool BackEdge { get; } = false;
		
	    public bool FallThrough { get;  } = false;
	    
	    public bool Outgoing { get; } = false;

	    internal AbstractBasicBlockEdge( 
		    BNBasicBlockEdge native ,
		    T_BASICBLOCK source,
		    T_BASICBLOCK target,
		    bool outgoing
		)
	    {
		    this.Type = native.type;

		    this.Source = source;
		    this.Target = target;
		    
		    this.BackEdge = native.backEdge;
		    this.FallThrough = native.fallThrough;
		    
		    this.Outgoing = outgoing;
	    }

	    public override bool Equals(object? other)
	    {
		    return Equals(other as T_SELF);
	    }

	    public bool Equals(T_SELF? other)
	    {
		    if (other is null)
		    {
			    return false;
		    }

		    if (ReferenceEquals(this , other))
		    {
			    return true;
		    }

		    if (this.Source.Equals(other.Source) 
		        && this.Target.Equals(other.Target)
		        && this.Type == other.Type
		        && this.BackEdge == other.BackEdge
		        && this.FallThrough == other.FallThrough
		       )
		    {
			    return true;
		    }
			
		    return false;
	    }

	    public override int GetHashCode()
	    {
		    return HashCode.Combine<uint,int,int,bool,bool>(
			    (uint)Type, 
			    Source.GetHashCode(), 
			    Target.GetHashCode(), 
			    BackEdge,
			    FallThrough
		    );
	    }

	    public static bool operator ==(
		    AbstractBasicBlockEdge<T_SELF,T_BASICBLOCK>? left,
		    AbstractBasicBlockEdge<T_SELF,T_BASICBLOCK>? right)
	    {
		    if (left is null)
		    {
			    return right is null;
		    }
			
		    return left.Equals(right);
	    }

	    public static bool operator !=(
		    AbstractBasicBlockEdge<T_SELF,T_BASICBLOCK>? left, 
		    AbstractBasicBlockEdge<T_SELF,T_BASICBLOCK>? right)
	    {
		    return !(left == right);
	    }

	    public int CompareTo(T_SELF? other)
	    {
		    if (other is null)
		    {
			    return 1;
		    }
			
		    int result = this.Type.CompareTo(other.Type);

		    if (0 == result)
		    {
			    result = this.Source.CompareTo(other.Source);
		    }

		    if (0 == result)
		    {
			    result = this.Target.CompareTo(other.Target);
		    }
			
		    if (0 == result)
		    {
			    result = this.BackEdge.CompareTo(other.BackEdge);
		    }
			
		    if (0 == result)
		    {
			    result = this.FallThrough.CompareTo(other.FallThrough);
		    }
			
		    return result;
	    }
    }
	
    public sealed class BasicBlockEdge : AbstractBasicBlockEdge<BasicBlockEdge,BasicBlock>
    {
	    internal BasicBlockEdge( 
		    BNBasicBlockEdge native ,
		    BasicBlock source,
		    BasicBlock target ,
		    bool outgoing
		) : base(native , source , target , outgoing)
	    {
		    
	    }

	    internal static BasicBlockEdge FromNativeEx(
		    BNBasicBlockEdge native ,
		    BasicBlock me,
		    bool outgoing
		)
	    {
		    if (outgoing)
		    {
			    return new BasicBlockEdge(
				    native, 
				    me , 
				    BasicBlock.MustNewFromHandle(native.target),
				    outgoing
				);
		    }
		    else
		    {
			   
			    return new BasicBlockEdge(
				    native, 
				    BasicBlock.MustNewFromHandle(native.target) , 
				    me,
				    outgoing
				);
		    }
	    }
    }
}