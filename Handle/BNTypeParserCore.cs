using System;

namespace BinaryNinja
{
	public abstract partial class TypeParser
	{
		private sealed class CoreTypeParser : TypeParser
		{
			internal CoreTypeParser(IntPtr handle)
				: base(handle)
			{
			}

			public override string? GetOptionText(TypeParserOption option, string value)
			{
				return this.GetOptionTextCore(option, value);
			}

			public override QualifiedNameAndType? ParseTypeString(
				string source,
				Platform platform,
				TypeContainer? existingTypes,
				out TypeParserError[] errors
			)
			{
				return this.ParseTypeStringCore(
					source,
					platform,
					existingTypes,
					out errors
				);
			}

			public override TypeParserResult? ParseTypesFromSource(
				string source,
				string fileName,
				Platform platform,
				TypeContainer? existingTypes,
				string[]? options,
				string[]? includeDirs,
				string autoTypeSource,
				out TypeParserError[] errors
			)
			{
				return this.ParseTypesFromSourceCore(
					source,
					fileName,
					platform,
					existingTypes,
					options,
					includeDirs,
					autoTypeSource,
					out errors
				);
			}

			public override string? PreprocessSource(
				string source,
				string fileName,
				Platform platform,
				TypeContainer? existingTypes,
				string[]? options,
				string[]? includeDirs,
				out TypeParserError[] errors
			)
			{
				return this.PreprocessSourceCore(
					source,
					fileName,
					platform,
					existingTypes,
					options,
					includeDirs,
					out errors
				);
			}
		}
	}
}
