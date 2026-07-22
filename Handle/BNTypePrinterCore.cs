using System;

namespace BinaryNinja
{
	public abstract partial class TypePrinter
	{
		private sealed class CoreTypePrinter : TypePrinter
		{
			internal CoreTypePrinter(IntPtr handle)
				: base(handle)
			{
			}

			public override TypeDefinitionLine[] GetTypeLines(
				BinaryNinja.Type type,
				TypeContainer types,
				QualifiedName name,
				int paddingCols,
				bool collapsed,
				TokenEscapingType escaping
			)
			{
				return this.GetTypeLinesCore(
					type, types, name, paddingCols, collapsed, escaping
				);
			}

			public override string? GetTypeString(
				BinaryNinja.Type type,
				Platform? platform,
				QualifiedName name,
				TokenEscapingType escaping
			)
			{
				return this.GetTypeStringCore(type, platform, name, escaping);
			}

			public override string? GetTypeStringAfterName(
				BinaryNinja.Type type,
				Platform? platform,
				TokenEscapingType escaping
			)
			{
				return this.GetTypeStringAfterNameCore(type, platform, escaping);
			}

			public override string? GetTypeStringBeforeName(
				BinaryNinja.Type type,
				Platform? platform,
				TokenEscapingType escaping
			)
			{
				return this.GetTypeStringBeforeNameCore(type, platform, escaping);
			}

			public override InstructionTextToken[] GetTypeTokens(
				BinaryNinja.Type type,
				Platform? platform,
				QualifiedName name,
				byte baseConfidence,
				TokenEscapingType escaping
			)
			{
				return this.GetTypeTokensCore(
					type, platform, name, baseConfidence, escaping
				);
			}

			public override InstructionTextToken[] GetTypeTokensAfterName(
				BinaryNinja.Type type,
				Platform? platform,
				byte baseConfidence,
				BinaryNinja.Type? parentType,
				TokenEscapingType escaping
			)
			{
				return this.GetTypeTokensAfterNameCore(
					type, platform, baseConfidence, parentType, escaping
				);
			}

			public override InstructionTextToken[] GetTypeTokensBeforeName(
				BinaryNinja.Type type,
				Platform? platform,
				byte baseConfidence,
				BinaryNinja.Type? parentType,
				TokenEscapingType escaping
			)
			{
				return this.GetTypeTokensBeforeNameCore(
					type, platform, baseConfidence, parentType, escaping
				);
			}

			public override string? PrintAllTypes(
				QualifiedName[] names,
				BinaryNinja.Type[] types,
				BinaryView data,
				int paddingCols,
				TokenEscapingType escaping
			)
			{
				return this.PrintAllTypesCore(
					names, types, data, paddingCols, escaping
				);
			}
		}
	}
}
