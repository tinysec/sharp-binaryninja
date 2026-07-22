using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	/// <summary>
	/// Represents a registered type printer that renders Binary Ninja type objects into
	/// human-readable text. TypePrinter handles are always borrowed because the native
	/// engine's global registry manages their lifetime.
	/// </summary>
	public abstract partial class TypePrinter : AbstractSafeHandle<TypePrinter>
	{
		private readonly string? registrationName;

		/// <summary>Creates an unregistered custom type printer.</summary>
		/// <param name="name">The unique registration name.</param>
		protected TypePrinter(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private TypePrinter(IntPtr handle)
			: base(handle, false)
		{
		}

		internal static TypePrinter? BorrowHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreTypePrinter(handle);
		}

		internal static TypePrinter MustBorrowHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new CoreTypePrinter(handle);
		}

		protected override bool ReleaseHandle()
		{
			return true;
		}

		/// <summary>Gets the unique registered name of this type printer.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				IntPtr raw = NativeMethods.BNGetTypePrinterName(this.handle);
				return UnsafeUtils.TakeAnsiString(raw) ?? string.Empty;
			}
		}

		/// <summary>Looks up a registered type printer by name.</summary>
		/// <param name="name">The registered printer name.</param>
		/// <returns>The matching printer, or null if the name is not registered.</returns>
		public static TypePrinter? GetByName(string name)
		{
			return TypePrinter.BorrowHandle(
				NativeMethods.BNGetTypePrinterByName(name ?? string.Empty)
			);
		}

		/// <summary>Gets the type printer selected by the analysis settings.</summary>
		/// <returns>The configured default printer, or null if it is unavailable.</returns>
		public static TypePrinter? GetDefault()
		{
			using (Settings settings = new Settings())
			{
				string name = settings.GetString(
					"analysis.types.printerName",
					null,
					null,
					out SettingsScope scope
				);
				if (string.IsNullOrEmpty(name))
				{
					return null;
				}

				return TypePrinter.GetByName(name);
			}
		}

		/// <summary>Gets the complete definition lines for a type.</summary>
		/// <param name="type">The type to render.</param>
		/// <param name="types">The type container used to resolve references.</param>
		/// <param name="name">The qualified type name.</param>
		/// <param name="paddingCols">The number of padding columns.</param>
		/// <param name="collapsed">Whether to collapse the rendered type.</param>
		/// <param name="escaping">The token escaping mode.</param>
		/// <returns>The rendered definition lines.</returns>
		public abstract TypeDefinitionLine[] GetTypeLines(
			BinaryNinja.Type type,
			TypeContainer types,
			QualifiedName name,
			int paddingCols,
			bool collapsed,
			TokenEscapingType escaping
		);

		/// <summary>Gets the complete string representation of a type.</summary>
		/// <param name="type">The type to render.</param>
		/// <param name="platform">The optional platform context.</param>
		/// <param name="name">The qualified type name.</param>
		/// <param name="escaping">The token escaping mode.</param>
		/// <returns>The rendered type string.</returns>
		public virtual string? GetTypeString(
			BinaryNinja.Type type,
			Platform? platform,
			QualifiedName name,
			TokenEscapingType escaping
		)
		{
			string before = this.GetTypeStringBeforeName(type, platform, escaping)
				?? string.Empty;
			string qualifiedName = TypePrinter.FormatQualifiedName(name, escaping);
			string after = this.GetTypeStringAfterName(type, platform, escaping)
				?? string.Empty;
			bool needsSpaceBeforeName =
				0 < before.Length && 0 < qualifiedName.Length &&
				' ' != before[before.Length - 1] && ' ' != qualifiedName[0];
			bool needsSpaceBeforeSuffix =
				0 < before.Length && 0 < after.Length &&
				' ' != before[before.Length - 1] && ' ' != after[0];
			if (needsSpaceBeforeName || needsSpaceBeforeSuffix)
			{
				return before + " " + qualifiedName + after;
			}

			return before + qualifiedName + after;
		}

		/// <summary>Gets the portion of a type string after the type name.</summary>
		/// <param name="type">The type to render.</param>
		/// <param name="platform">The optional platform context.</param>
		/// <param name="escaping">The token escaping mode.</param>
		/// <returns>The rendered suffix.</returns>
		public virtual string? GetTypeStringAfterName(
			BinaryNinja.Type type,
			Platform? platform,
			TokenEscapingType escaping
		)
		{
			return TypePrinter.JoinTokenText(
				this.GetTypeTokensAfterName(
					type, platform, Core.MaxConfidence, null, escaping
				)
			);
		}

		/// <summary>Gets the portion of a type string before the type name.</summary>
		/// <param name="type">The type to render.</param>
		/// <param name="platform">The optional platform context.</param>
		/// <param name="escaping">The token escaping mode.</param>
		/// <returns>The rendered prefix.</returns>
		public virtual string? GetTypeStringBeforeName(
			BinaryNinja.Type type,
			Platform? platform,
			TokenEscapingType escaping
		)
		{
			return TypePrinter.JoinTokenText(
				this.GetTypeTokensBeforeName(
					type, platform, Core.MaxConfidence, null, escaping
				)
			);
		}

		/// <summary>Gets the complete token representation of a type.</summary>
		/// <param name="type">The type to render.</param>
		/// <param name="platform">The optional platform context.</param>
		/// <param name="name">The qualified type name.</param>
		/// <param name="baseConfidence">The base token confidence.</param>
		/// <param name="escaping">The token escaping mode.</param>
		/// <returns>The rendered tokens.</returns>
		public virtual InstructionTextToken[] GetTypeTokens(
			BinaryNinja.Type type,
			Platform? platform,
			QualifiedName name,
			byte baseConfidence,
			TokenEscapingType escaping
		)
		{
			InstructionTextToken[] before = this.GetTypeTokensBeforeName(
				type, platform, baseConfidence, null, escaping
			);
			InstructionTextToken[] after = this.GetTypeTokensAfterName(
				type, platform, baseConfidence, null, escaping
			);
			List<InstructionTextToken> result = new List<InstructionTextToken>(before);
			if (0 < before.Length && 0 < after.Length)
			{
				string beforeText = before[before.Length - 1].Text;
				string afterText = after[0].Text;
				if (0 < beforeText.Length && 0 < afterText.Length &&
					' ' != beforeText[beforeText.Length - 1] &&
					'*' != beforeText[beforeText.Length - 1] &&
					'&' != beforeText[beforeText.Length - 1] &&
					' ' != afterText[0] &&
					TypeClass.FunctionTypeClass != type.TypeClass)
				{
					result.Add(new InstructionTextToken(
						InstructionTextTokenType.TextToken, " "
					));
				}
			}

			result.AddRange(after);
			return result.ToArray();
		}

		/// <summary>Gets the tokens that appear after the type name.</summary>
		public abstract InstructionTextToken[] GetTypeTokensAfterName(
			BinaryNinja.Type type,
			Platform? platform,
			byte baseConfidence,
			BinaryNinja.Type? parentType,
			TokenEscapingType escaping
		);

		/// <summary>Gets the tokens that appear before the type name.</summary>
		public abstract InstructionTextToken[] GetTypeTokensBeforeName(
			BinaryNinja.Type type,
			Platform? platform,
			byte baseConfidence,
			BinaryNinja.Type? parentType,
			TokenEscapingType escaping
		);

		/// <summary>Gets every registered type printer.</summary>
		/// <returns>The registered printers as borrowed handles.</returns>
		public static unsafe TypePrinter[] GetList()
		{
			ulong count = 0;
			IntPtr arrayPointer = NativeMethods.BNGetTypePrinterList((IntPtr)(&count));
			return UnsafeUtils.TakeHandleArray<TypePrinter>(
				arrayPointer,
				count,
				TypePrinter.MustBorrowHandle,
				NativeMethods.BNFreeTypePrinterList
			);
		}

		private static string FormatQualifiedName(
			QualifiedName name,
			TokenEscapingType escaping
		)
		{
			if (null == name || null == name.Name || 0 == name.Name.Length)
			{
				return string.Empty;
			}

			string[] escaped = new string[name.Name.Length];
			for (int i = 0; i < name.Name.Length; i++)
			{
				escaped[i] = Core.EscapeTypeName(name.Name[i], escaping);
			}

			string join = string.IsNullOrEmpty(name.Join) ? "::" : name.Join;
			return string.Join(join, escaped);
		}

		private static string JoinTokenText(InstructionTextToken[]? tokens)
		{
			InstructionTextToken[] safeTokens =
				tokens ?? Array.Empty<InstructionTextToken>();
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			foreach (InstructionTextToken token in safeTokens)
			{
				if (null != token)
				{
					result.Append(token.Text);
				}
			}

			return result.ToString();
		}
	}
}
