using System;

namespace BinaryNinja
{
	/// <summary>
	/// A decoded string annotation and the string encoding type selected by analysis.
	/// </summary>
	public sealed class StringAnnotation
	{
		public string Value { get; }

		public StringType Type { get; }

		public StringAnnotation(string value, StringType type)
		{
			if (null == value)
			{
				throw new ArgumentNullException(nameof(value));
			}

			this.Value = value;
			this.Type = type;
		}
	}
}
