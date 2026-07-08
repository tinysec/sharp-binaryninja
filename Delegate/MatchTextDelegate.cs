using System;

namespace BinaryNinja
{
	public delegate bool MatchTextDelegate
	(
		ulong address ,
		string text,
		LinearDisassemblyLine line
	);

	internal static partial class NativeDelegates
	{
		// bool (*matchCallback)(void* matchCtxt, uint64_t addr, const char* match, BNLinearDisassemblyLine* line)
		public delegate bool MatchTextDelegate(
			IntPtr matchCtxt,
			ulong address,
			IntPtr match ,
			IntPtr line
		);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a public MatchTextDelegate into the native match-callback shape. The BN core hands
		// the callback an OWNED BNLinearDisassemblyLine* per match (the official Python binding
		// frees it via _LinearDisassemblyLine_convertor -> BNFreeLinearDisassemblyLines). The line
		// is fully copied into managed objects by MustFromNativePointer, so freeing the native line
		// afterward is safe; the earlier form never freed it and leaked one line per match. The
		// matched text is a borrowed const char* (core-owned) and is only read, never freed.
		internal sealed class MatchTextContext
		{
			private readonly MatchTextDelegate m_callback;

			internal MatchTextContext(MatchTextDelegate callback)
			{
				this.m_callback = callback;
			}

			internal bool OnMatch(IntPtr matchCtxt, ulong address, IntPtr text, IntPtr line)
			{
				LinearDisassemblyLine copy = LinearDisassemblyLine.MustFromNativePointer(line);

				NativeMethods.BNFreeLinearDisassemblyLines(line, 1);

				return this.m_callback(
					address ,
					UnsafeUtils.ReadUtf8String(text),
					copy
				);
			}
		}

		internal static NativeDelegates.MatchTextDelegate WrapMatchTextDelegate(
			MatchTextDelegate callback)
		{
			MatchTextContext context = new MatchTextContext(callback);

			return new NativeDelegates.MatchTextDelegate(context.OnMatch);
		}
	}
}
