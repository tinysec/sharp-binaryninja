using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNLinearDisassemblyLine 
	{
		/// <summary>
		/// BNLinearDisassemblyLineType type
		/// </summary>
		internal LinearDisassemblyLineType type;
		
		/// <summary>
		/// BNFunction* function
		/// </summary>
		internal IntPtr function;
		
		/// <summary>
		/// BNBasicBlock* block
		/// </summary>
		internal IntPtr block;
		
		/// <summary>
		/// BNDisassemblyTextLine contents
		/// </summary>
		internal BNDisassemblyTextLine contents;
	}

    public sealed class LinearDisassemblyLine 
	    : INativeWrapperEx<BNLinearDisassemblyLine>,
		IEquatable<LinearDisassemblyLine>, 
	    IComparable<LinearDisassemblyLine>
    {
		public LinearDisassemblyLineType Type { get;  } = LinearDisassemblyLineType.BlankLineType;
		
		public Function? Function { get;  } = null;
		
		public BasicBlock? Block { get;  } = null;
		
		public DisassemblyTextLine Contents { get;  } = new DisassemblyTextLine();
		
		public LinearDisassemblyLine() 
		{
		    
		}
		
		internal static LinearDisassemblyLine MustFromNativePointer(IntPtr pointer)
		{
			if (IntPtr.Zero == pointer)
			{
				throw new NullReferenceException(nameof(pointer));
			}
			
			return LinearDisassemblyLine.FromNative(Marshal.PtrToStructure<BNLinearDisassemblyLine>(pointer));
		}
		
		internal static LinearDisassemblyLine FromNative(BNLinearDisassemblyLine native)
		{
			return new LinearDisassemblyLine(native);
		}
		
		internal LinearDisassemblyLine(BNLinearDisassemblyLine native)
		{
			this.Type = native.type;
			this.Function = ( IntPtr.Zero == native.function ? null : Function.MustNewFromHandle(native.function) );
			this.Block = ( IntPtr.Zero == native.block ? null : BasicBlock.MustNewFromHandle(native.block) );
			this.Contents = DisassemblyTextLine.FromNative(native.contents);
		}
		
		public BNLinearDisassemblyLine ToNativeEx(ScopedAllocator allocator)
		{
			return new BNLinearDisassemblyLine()
			{
				type = this.Type ,
				function = ( null == this.Function ? IntPtr.Zero : this.Function.DangerousGetHandle() ) ,
				block = ( null == this.Block ? IntPtr.Zero : this.Block.DangerousGetHandle() ) ,
				contents = this.Contents.ToNativeEx(allocator),
			};
		}

		public override string ToString()
		{
			return this.Contents.ToString();
		}
		
		public override bool Equals(object? rawOther)
		{
			LinearDisassemblyLine? other = rawOther as LinearDisassemblyLine;
			
			if (other is null)
			{
				return false;
			}

			return this.Equals(other);
		}

		public bool Equals(LinearDisassemblyLine? other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this , other))
			{
				return true;
			}

			if (null != this.Function && null != other.Function)
			{
				if (this.Function != other.Function)
				{
					return false;
				}
			}
			
			if (null != this.Block && null != other.Block)
			{
				if (this.Block != other.Block)
				{
					return false;
				}
			}

			if (this.Type != other.Type)
			{
				return false;
			}
			
			return this.Contents.Equals(other.Contents);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals")]
		public override int GetHashCode()
		{
			return this.Contents.GetHashCode();
		}

		public static bool operator ==(LinearDisassemblyLine? left, LinearDisassemblyLine? right)
		{
			if (left is null)
			{
				return right is null;
			}
			
			return left.Equals(right);
		}

		public static bool operator !=(LinearDisassemblyLine? left, LinearDisassemblyLine? right)
		{
			return !(left == right);
		}

		public int CompareTo(LinearDisassemblyLine? other)
		{
			if (other is null)
			{
				return 1;
			}
			
			return this.Contents.CompareTo(other.Contents);
		}
    }
}