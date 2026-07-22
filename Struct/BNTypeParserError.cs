using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNTypeParserError
	{
		/// <summary>
		/// BNTypeParserErrorSeverity severity
		/// </summary>
		public TypeParserErrorSeverity severity;
		
		/// <summary>
		/// const char* message
		/// </summary>
		public IntPtr message;
		
		/// <summary>
		/// const char* fileName
		/// </summary>
		public IntPtr fileName;
		
		/// <summary>
		/// uint64_t line
		/// </summary>
		public ulong line;
		
		/// <summary>
		/// uint64_t column
		/// </summary>
		public ulong column;
	}

    public sealed class TypeParserError 
    {
		public TypeParserErrorSeverity Severity { get; set; } = TypeParserErrorSeverity.IgnoredSeverity;
		
		public string Message { get; set; } = string.Empty;
		
		public string FileName { get; set; } = string.Empty;
		
		public ulong Line { get; set; } = 0;
		
		public ulong Column { get; set; } = 0;
		
		public TypeParserError()
		{
		}

		public TypeParserError(TypeParserErrorSeverity severity, string message)
			: this(severity, message, string.Empty, 0, 0)
		{
		}

		public TypeParserError(
			TypeParserErrorSeverity severity,
			string message,
			string fileName,
			ulong line,
			ulong column)
		{
			this.Severity = severity;
			this.Message = message ?? string.Empty;
			this.FileName = fileName ?? string.Empty;
			this.Line = line;
			this.Column = column;
		}

		internal static TypeParserError FromNative(BNTypeParserError raw)
		{
			return new TypeParserError()
			{
				Severity = raw.severity ,
				Message = UnsafeUtils.ReadAnsiString(raw.message) ,
				FileName = UnsafeUtils.ReadAnsiString(raw.fileName) ,
				Line = raw.line ,
				Column = raw.column
			};
		}

		internal BNTypeParserError ToNativeEx(ScopedAllocator allocator)
		{
			return new BNTypeParserError
			{
				severity = this.Severity,
				message = allocator.AllocUtf8String(this.Message),
				fileName = allocator.AllocUtf8String(this.FileName),
				line = this.Line,
				column = this.Column
			};
		}
    }
}
