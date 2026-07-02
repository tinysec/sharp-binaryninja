using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNBoolWithConfidence 
	{
		/// <summary>
		/// bool value
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool value;
		
		/// <summary>
		/// uint8_t confidence
		/// </summary>
		public byte confidence;
	}

    public sealed class BoolWithConfidence 
    {
		public bool Value { get; set; } = false;
		
		public byte Confidence { get; set; } = 0;
		
		public BoolWithConfidence() 
		{
		    
		}
		
		public BoolWithConfidence(bool value = false , byte confidence = 0) 
		{
		    this.Value = value;
		    
		    this.Confidence = confidence;
		}

		internal static BoolWithConfidence FromNative(BNBoolWithConfidence raw)
		{
			return new BoolWithConfidence()
			{
				Value = raw.value , 
				Confidence = raw.confidence
			};
		}

		internal BNBoolWithConfidence ToNative()
		{
			return new BNBoolWithConfidence()
			{
				value = Value,
				confidence = Confidence
			};
		}
		
		public static implicit operator BoolWithConfidence(bool value)
		{
			return new BoolWithConfidence(value);
		}

		public static implicit operator bool(BoolWithConfidence? kind)
		{
			if (kind == null)
			{
				return false;
			}

			return kind.Value;
		}
    }
}