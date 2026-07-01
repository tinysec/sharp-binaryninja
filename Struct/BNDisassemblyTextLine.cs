using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNDisassemblyTextLine 
	{
		/// <summary>
		/// uint64_t addr
		/// </summary>
		internal ulong addr;
		
		/// <summary>
		/// uint64_t instrIndex
		/// </summary>
		internal ulong instrIndex;
		
		/// <summary>
		/// BNInstructionTextToken* tokens
		/// </summary>
		internal IntPtr tokens;
		
		/// <summary>
		/// uint64_t count
		/// </summary>
		internal ulong count;
		
		/// <summary>
		/// BNHighlightColor highlight
		/// </summary>
		internal BNHighlightColor highlight;
		
		/// <summary>
		/// BNTag** tags
		/// </summary>
		internal IntPtr tags;
		
		/// <summary>
		/// uint64_t tagCount
		/// </summary>
		internal ulong tagCount;
		
		/// <summary>
		/// BNDisassemblyTextLineTypeInfo typeInfo
		/// </summary>
		internal BNDisassemblyTextLineTypeInfo typeInfo;
	}

    public sealed class DisassemblyTextLine : INativeWrapperEx<BNDisassemblyTextLine>,
		IEquatable<DisassemblyTextLine>, 
	    IComparable<DisassemblyTextLine>
    {
		public ulong Address { get;  } = 0;
		
		public ulong InstructionIndex { get;  } = 0;
		
		public InstructionTextToken[] Tokens { get;  } = Array.Empty<InstructionTextToken>();
		
		public HighlightColor Highlight { get;  } = new HighlightColor();
		
		public Tag[] Tags { get;  } = Array.Empty<Tag>();
		
		public DisassemblyTextLineTypeInfo TypeInfo { get;  } = new DisassemblyTextLineTypeInfo();

		internal DisassemblyTextLine()
		{
			
		}
		
		internal DisassemblyTextLine(BNDisassemblyTextLine native)
		{
			this.Address = native.addr;
			this.InstructionIndex = native.instrIndex;

			this.Tokens = UnsafeUtils.ReadStructArray<BNInstructionTextToken , InstructionTextToken>(
				native.tokens ,
				native.count ,
				InstructionTextToken.FromNative 
			);

			this.Highlight = HighlightColor.FromNative(native.highlight);

			this.Tags = UnsafeUtils.ReadHandleArray<Tag>(
				native.tags ,
				native.tagCount ,
				Tag.MustNewFromHandle
			);

			this.TypeInfo = DisassemblyTextLineTypeInfo.FromNative(native.typeInfo);
		}

		internal static DisassemblyTextLine FromNative(BNDisassemblyTextLine native)
		{
			return new DisassemblyTextLine(native);
		}
	
		public BNDisassemblyTextLine ToNativeEx(ScopedAllocator allocator)
		{
			return new BNDisassemblyTextLine
			{
				addr = this.Address ,
				instrIndex = this.InstructionIndex ,
				tokens = ( 0 == this.Tokens.Length ? 
						IntPtr.Zero : 
						allocator.AllocStructArray<BNInstructionTextToken>(
							allocator.ConvertToNativeArrayEx<BNInstructionTextToken,InstructionTextToken>(this.Tokens)
						) 
					) ,
				highlight = this.Highlight.ToNative() ,
				tags = allocator.AllocHandleArray<Tag>(this.Tags) ,
				typeInfo = this.TypeInfo.ToNative()
			};
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach (InstructionTextToken token in this.Tokens)
			{
				builder.Append(token.ToString());
			}
			
			return builder.ToString();
		}
		
		public override bool Equals(object? rawOther)
	    {
		    InstructionTextToken? other = rawOther as InstructionTextToken;
			
		    if (other is null)
		    {
			    return false;
		    }

		    return this.Equals(other);
	    }

	    public bool Equals(DisassemblyTextLine? other)
	    {
		    if (other is null)
		    {
			    return false;
		    }

		    if (ReferenceEquals(this , other))
		    {
			    return true;
		    }

		    if (this.Address != other.Address)
		    {
			    return false;
		    }
		    
		    if (this.InstructionIndex != other.InstructionIndex)
		    {
			    return false;
		    }
		    
		    if (this.Tokens.Length != other.Tokens.Length)
		    {
			    return false;
		    }

		    for (int i = 0; i < this.Tokens.Length; i++)
		    {
			    if (!this.Tokens[i].Equals(other.Tokens[i]))
			    {
				    return false;
			    }
		    }
		    
		    return true;
	    }

	    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals")]
	    public override int GetHashCode()
	    {
		    return HashCode.Combine<ulong,ulong>(
			    this.Address,
			    this.InstructionIndex
			);
	    }

	    public static bool operator ==(DisassemblyTextLine? left, DisassemblyTextLine? right)
	    {
		    if (left is null)
		    {
			    return right is null;
		    }
			
		    return left.Equals(right);
	    }

	    public static bool operator !=(DisassemblyTextLine? left, DisassemblyTextLine? right)
	    {
		    return !(left == right);
	    }

	    public int CompareTo(DisassemblyTextLine? other)
	    {
		    if (other is null)
		    {
			    return 1;
		    }
		    
		    int result = this.Address.CompareTo(other.Address);

		    if (0 == result)
		    {
			    result = this.InstructionIndex.CompareTo(other.InstructionIndex);
		    }
		    
		    return result;
	    }
    }
}