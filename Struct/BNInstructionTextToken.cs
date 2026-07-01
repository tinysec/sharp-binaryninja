
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNInstructionTextToken
	{
		/// <summary>
		/// 
		/// BNInstructionTextTokenType type
		/// </summary>
		internal InstructionTextTokenType type;
		
		/// <summary>
		/// 
		/// const char* text
		/// </summary>
		internal IntPtr text;
		
		/// <summary>
		/// 
		/// uint64_t value
		/// </summary>
		internal ulong value;
		
		/// <summary>
		/// 
		/// uint64_t width
		/// </summary>
		internal ulong width;
		
		/// <summary>
		/// 
		/// uint64_t size
		/// </summary>
		internal ulong size;
		
		/// <summary>
		/// 
		/// uint64_t operand
		/// </summary>
		internal ulong operand;
		
		/// <summary>
		/// 
		/// BNInstructionTextTokenContext context
		/// </summary>
		internal InstructionTextTokenContext context;
		
		/// <summary>
		/// 
		/// uint8_t confidence
		/// </summary>
		internal byte confidence;
		
		/// <summary>
		/// 
		/// uint64_t address
		/// </summary>
		internal ulong address;
		
		/// <summary>
		/// 
		/// const char** typeNames
		/// </summary>
		internal IntPtr typeNames;
		
		/// <summary>
		/// 
		/// uint64_t namesCount
		/// </summary>
		internal ulong namesCount;
		
		/// <summary>
		/// 
		/// uint64_t exprIndex
		/// </summary>
		internal ulong exprIndex;
	}
	
	/// <summary>
	/// 
	/// </summary>
    public sealed class InstructionTextToken 
		: INativeWrapperEx<BNInstructionTextToken>,
			IEquatable<InstructionTextToken>, 
			IComparable<InstructionTextToken>
    {
	    public InstructionTextTokenType Type { get;  } = InstructionTextTokenType.TextToken;
	    
		public string Text { get;  } = string.Empty;
		
		public ulong Value { get;  } = 0;
		
		public ulong Width { get;  } = 0;
	
		public ulong Size { get; } = 0;
	
		public ulong Operand { get;  } = 0;
		
		public InstructionTextTokenContext Context { get; } = InstructionTextTokenContext.NoTokenContext;
	
		public byte Confidence { get;  } = 0;
		
		public ulong Address { get;  } = 0;
		
		public string[] TypeNames { get;  } = Array.Empty<string>();
	
		public ulong ExpressionIndex { get;  } = 0;
		
		public InstructionTextToken( ) 
		{
		    
		}
		
		public InstructionTextToken( BNInstructionTextToken native)
		{
			this.Type = native.type;

			this.Text = UnsafeUtils.ReadUtf8String(native.text);

			this.Value = native.value;

			this.Width = native.width;
			    
			this.Size = native.size;

			this.Operand = native.operand;

			this.Context = native.context;

			this.Confidence = native.confidence;

			this.Address = native.address;

			this.TypeNames = UnsafeUtils.ReadAnsiStringArray(native.typeNames , native.namesCount);

			this.ExpressionIndex = native.exprIndex;
		}
		
	    internal static InstructionTextToken FromNative(BNInstructionTextToken native)
	    {
		    return new InstructionTextToken(native);
	    }
	    
	    public BNInstructionTextToken ToNativeEx(ScopedAllocator allocator)
	    {
		    return new BNInstructionTextToken()
		    {
			    type = this.Type,

			    text = allocator.AllocAnsiString(this.Text),

			    value = this.Value,

			    width = this.Width,
			    
			    size = this.Size,

			    operand = this.Operand,

			    context = this.Context,

			    confidence = this.Confidence,

			    address = this.Address,

			    typeNames = ( 0 == this.TypeNames.Length ? IntPtr.Zero : allocator.AllocAnsiStringArray(this.TypeNames) ) ,
			    
			    namesCount = (ulong)this.TypeNames.Length,

			    exprIndex = this.ExpressionIndex
		    };
	    }

	    public override string ToString()
	    {
		    return this.Text;
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

	    public bool Equals(InstructionTextToken? other)
	    {
		    if (other is null)
		    {
			    return false;
		    }

		    if (ReferenceEquals(this , other))
		    {
			    return true;
		    }

		    if (this.Text != other.Text)
		    {
			    return false;
		    }
			
		    if (this.Value != other.Value)
		    {
			    return false;
		    }
		    
		    if (this.Width != other.Width)
		    {
			    return false;
		    }
		    
		    if (this.Size != other.Size)
		    {
			    return false;
		    }
		    
		    if (this.Operand != other.Operand)
		    {
			    return false;
		    }
		    
		    if (this.Context != other.Context)
		    {
			    return false;
		    }
		    
		    if (this.Confidence != other.Confidence)
		    {
			    return false;
		    }
		    
		    if (this.Address != other.Address)
		    {
			    return false;
		    }
		    
		    if (this.TypeNames.Length != other.TypeNames.Length)
		    {
			    return false;
		    }

		    for (int i = 0; i < this.TypeNames.Length; i++)
		    {
			    if (this.TypeNames[i] != other.TypeNames[i])
			    {
				    return false;
			    }
		    }
			
		    return this.ExpressionIndex == other.ExpressionIndex;
	    }

	    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals")]
	    public override int GetHashCode()
	    {
		    return HashCode.Combine<int,ulong,ulong,ulong,ulong,ulong,ulong,ulong>(
			    this.Text.GetHashCode(),
			    this.Value,
			    this.Width,
			    this.Size,
			    this.Operand,
			    (uint)this.Context,
			    this.Address,
			    this.ExpressionIndex
			);
	    }

	    public static bool operator ==(InstructionTextToken? left, InstructionTextToken? right)
	    {
		    if (left is null)
		    {
			    return right is null;
		    }
			
		    return left.Equals(right);
	    }

	    public static bool operator !=(InstructionTextToken? left, InstructionTextToken? right)
	    {
		    return !(left == right);
	    }

	    public int CompareTo(InstructionTextToken? other)
	    {
		    if (other is null)
		    {
			    return 1;
		    }
		    
		    int result = this.Address.CompareTo(other.Address);

		    if (0 == result)
		    {
			    result = this.ExpressionIndex.CompareTo(other.ExpressionIndex);
		    }
			
		    if (0 == result)
		    {
			    result = this.Operand.CompareTo(other.Operand);
		    }
		    
		    if (0 == result)
		    {
			    result = this.Size.CompareTo(other.Size);
		    }
		    
		    return result;
	    }
    }
}