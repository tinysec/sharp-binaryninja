using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNStringReference 
	{
		/// <summary>
		/// BNStringType type
		/// </summary>
		internal StringType type;
		
		/// <summary>
		/// uint64_t start
		/// </summary>
		internal ulong start;
		
		/// <summary>
		/// uint64_t length
		/// </summary>
		internal ulong length;
	}

    public sealed class StringReference : INativeWrapper<BNStringReference>
    {
	    private static readonly Dictionary<StringType, Encoding> s_decodings =
		    new Dictionary<StringType, Encoding>
		    {
			    { StringType.AsciiString, Encoding.ASCII },
			    { StringType.Utf8String, Encoding.UTF8 },
			    { StringType.Utf16String, Encoding.Unicode },
			    { StringType.Utf32String, Encoding.UTF32 },
		    };

	    public StringType Type { get; set; } = StringType.AsciiString;
	    
		public ulong Start { get; set; } = 0;
		
		public ulong Length { get; set; } = 0;

		/// <summary>
		/// Gets the binary view that owns this string reference, matching the official binding.
		/// Legacy manually constructed references may not have an associated view.
		/// </summary>
		public BinaryView? View { get; private set; }

		/// <summary>
		/// Gets the exact bytes covered by this string reference.
		/// </summary>
		public byte[] Raw
		{
			get
			{
				return this.RequireView().ReadData(this.Start, this.Length);
			}
		}

		/// <summary>
		/// Gets the decoded string value using the encoding selected by <see cref="Type"/>.
		/// Invalid byte sequences are replaced, matching Python's <c>errors='replace'</c> behavior.
		/// </summary>
		public string Value
		{
			get
			{
				return Decode(this.Raw, this.Type);
			}
		}
		
		public StringReference() 
		{
		    
		}

		public StringReference(
			BinaryView view,
			StringType type,
			ulong start,
			ulong length)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			this.View = view;
			this.Type = type;
			this.Start = start;
			this.Length = length;
		}

		internal static StringReference FromNative(BNStringReference raw, BinaryView view)
		{
			return new StringReference(view, raw.type, raw.start, raw.length);
		}

		internal static string Decode(byte[] raw, StringType type)
		{
			s_decodings.TryGetValue(type, out Encoding? foundEncoding);
			Encoding encoding = foundEncoding ?? Encoding.ASCII;
			return encoding.GetString(raw);
		}

		private BinaryView RequireView()
		{
			if (null == this.View)
			{
				throw new InvalidOperationException(
					"This StringReference is not associated with a BinaryView.");
			}

			return this.View;
		}

		public override string ToString()
		{
			if (null == this.View)
			{
				return "<" + this.Type + ": 0x" + this.Start.ToString("x")
					+ ", len 0x" + this.Length.ToString("x") + ">";
			}

			return this.Value;
		}
		
		public BNStringReference ToNative()
		{
			return new BNStringReference()
			{
				type = this.Type , 
				start = this.Start ,
				length = this.Length
			};
		}
    }
}
