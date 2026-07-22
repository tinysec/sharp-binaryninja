using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class TypePrinter
	{
		private readonly object tokenOutputLock = new object();

		private readonly Dictionary<IntPtr, ScopedAllocator> tokenOutputs =
			new Dictionary<IntPtr, ScopedAllocator>();

		private bool InvokeGetTypeTokens(
			IntPtr context, IntPtr type, IntPtr platform, IntPtr name,
			byte baseConfidence, TokenEscapingType escaping,
			IntPtr result, IntPtr resultCount
		)
		{
			TypePrinter.ResetListOutput(result, resultCount);
			BinaryNinja.Type? managedType = null;
			Platform? managedPlatform = null;
			try
			{
				managedType = BinaryNinja.Type.MustNewFromHandle(type);
				managedPlatform = Platform.NewFromHandle(platform);
				BNQualifiedName nativeName = Marshal.PtrToStructure<BNQualifiedName>(name);
				InstructionTextToken[] tokens = this.GetTypeTokens(
					managedType, managedPlatform, QualifiedName.FromNative(nativeName),
					baseConfidence, escaping
				);
				this.WriteTokenOutput(tokens, result, resultCount);
				return true;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypePrinter.GetTypeTokens: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedPlatform)
				{
					managedPlatform.Dispose();
				}

				if (null != managedType)
				{
					managedType.Dispose();
				}
			}
		}

		private bool InvokeGetTypeTokensBeforeName(
			IntPtr context, IntPtr type, IntPtr platform, byte baseConfidence,
			IntPtr parentType, TokenEscapingType escaping,
			IntPtr result, IntPtr resultCount
		)
		{
			return this.InvokeGetTypeTokensPart(
				false, type, platform, baseConfidence, parentType, escaping,
				result, resultCount
			);
		}

		private bool InvokeGetTypeTokensAfterName(
			IntPtr context, IntPtr type, IntPtr platform, byte baseConfidence,
			IntPtr parentType, TokenEscapingType escaping,
			IntPtr result, IntPtr resultCount
		)
		{
			return this.InvokeGetTypeTokensPart(
				true, type, platform, baseConfidence, parentType, escaping,
				result, resultCount
			);
		}

		private bool InvokeGetTypeTokensPart(
			bool afterName, IntPtr type, IntPtr platform, byte baseConfidence,
			IntPtr parentType, TokenEscapingType escaping,
			IntPtr result, IntPtr resultCount
		)
		{
			TypePrinter.ResetListOutput(result, resultCount);
			BinaryNinja.Type? managedType = null;
			BinaryNinja.Type? managedParentType = null;
			Platform? managedPlatform = null;
			try
			{
				managedType = BinaryNinja.Type.MustNewFromHandle(type);
				managedParentType = BinaryNinja.Type.NewFromHandle(parentType);
				managedPlatform = Platform.NewFromHandle(platform);
				InstructionTextToken[] tokens;
				if (afterName)
				{
					tokens = this.GetTypeTokensAfterName(
						managedType, managedPlatform, baseConfidence,
						managedParentType, escaping
					);
				}
				else
				{
					tokens = this.GetTypeTokensBeforeName(
						managedType, managedPlatform, baseConfidence,
						managedParentType, escaping
					);
				}

				this.WriteTokenOutput(tokens, result, resultCount);
				return true;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypePrinter.GetTypeTokensPart: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedPlatform)
				{
					managedPlatform.Dispose();
				}

				if (null != managedParentType)
				{
					managedParentType.Dispose();
				}

				if (null != managedType)
				{
					managedType.Dispose();
				}
			}
		}

		private bool InvokeGetTypeString(
			IntPtr context, IntPtr type, IntPtr platform, IntPtr name,
			TokenEscapingType escaping, IntPtr result
		)
		{
			Marshal.WriteIntPtr(result, IntPtr.Zero);
			BinaryNinja.Type? managedType = null;
			Platform? managedPlatform = null;
			try
			{
				managedType = BinaryNinja.Type.MustNewFromHandle(type);
				managedPlatform = Platform.NewFromHandle(platform);
				BNQualifiedName nativeName = Marshal.PtrToStructure<BNQualifiedName>(name);
				string? text = this.GetTypeString(
					managedType, managedPlatform,
					QualifiedName.FromNative(nativeName), escaping
				);
				this.WriteStringOutput(text, result);
				return null != text;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypePrinter.GetTypeString: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedPlatform)
				{
					managedPlatform.Dispose();
				}

				if (null != managedType)
				{
					managedType.Dispose();
				}
			}
		}

		private bool InvokeGetTypeStringBeforeName(
			IntPtr context, IntPtr type, IntPtr platform,
			TokenEscapingType escaping, IntPtr result
		)
		{
			return this.InvokeGetTypeStringPart(false, type, platform, escaping, result);
		}

		private bool InvokeGetTypeStringAfterName(
			IntPtr context, IntPtr type, IntPtr platform,
			TokenEscapingType escaping, IntPtr result
		)
		{
			return this.InvokeGetTypeStringPart(true, type, platform, escaping, result);
		}

		private bool InvokeGetTypeStringPart(
			bool afterName, IntPtr type, IntPtr platform,
			TokenEscapingType escaping, IntPtr result
		)
		{
			Marshal.WriteIntPtr(result, IntPtr.Zero);
			BinaryNinja.Type? managedType = null;
			Platform? managedPlatform = null;
			try
			{
				managedType = BinaryNinja.Type.MustNewFromHandle(type);
				managedPlatform = Platform.NewFromHandle(platform);
				string? text;
				if (afterName)
				{
					text = this.GetTypeStringAfterName(
						managedType, managedPlatform, escaping
					);
				}
				else
				{
					text = this.GetTypeStringBeforeName(
						managedType, managedPlatform, escaping
					);
				}

				this.WriteStringOutput(text, result);
				return null != text;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in TypePrinter.GetTypeStringPart: {0}",
					exception
				);
				return false;
			}
			finally
			{
				if (null != managedPlatform)
				{
					managedPlatform.Dispose();
				}

				if (null != managedType)
				{
					managedType.Dispose();
				}
			}
		}

		private void WriteTokenOutput(
			InstructionTextToken[]? tokens, IntPtr result, IntPtr resultCount
		)
		{
			InstructionTextToken[] safeTokens =
				tokens ?? Array.Empty<InstructionTextToken>();
			ScopedAllocator allocator = new ScopedAllocator();
			try
			{
				BNInstructionTextToken[] nativeTokens =
					allocator.ConvertToNativeArrayEx<
						BNInstructionTextToken, InstructionTextToken
					>(safeTokens);
				IntPtr nativeOutput = allocator.AllocStructArray(nativeTokens);
				if (IntPtr.Zero != nativeOutput)
				{
					lock (this.tokenOutputLock)
					{
						this.tokenOutputs.Add(nativeOutput, allocator);
					}
				}
				else
				{
					allocator.Dispose();
				}

				Marshal.WriteIntPtr(result, nativeOutput);
				Marshal.WriteInt64(resultCount, safeTokens.Length);
			}
			catch
			{
				allocator.Dispose();
				throw;
			}
		}

		private void WriteStringOutput(string? text, IntPtr result)
		{
			if (null == text)
			{
				return;
			}

			IntPtr nativeText = NativeMethods.BNAllocString(text);
			if (IntPtr.Zero == nativeText)
			{
				throw new InvalidOperationException("The core could not allocate printer text.");
			}

			Marshal.WriteIntPtr(result, nativeText);
		}

		private void InvokeFreeTokens(IntPtr context, IntPtr values, ulong count)
		{
			ScopedAllocator? allocator = null;
			lock (this.tokenOutputLock)
			{
				if (this.tokenOutputs.TryGetValue(values, out allocator))
				{
					this.tokenOutputs.Remove(values);
				}
			}

			if (null != allocator)
			{
				allocator.Dispose();
			}
		}

		private void InvokeFreeString(IntPtr context, IntPtr value)
		{
			NativeMethods.BNFreeString(value);
		}

		private static void ResetListOutput(IntPtr result, IntPtr resultCount)
		{
			Marshal.WriteIntPtr(result, IntPtr.Zero);
			Marshal.WriteInt64(resultCount, 0);
		}
	}
}
