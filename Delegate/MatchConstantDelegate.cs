using System;

namespace BinaryNinja
{
	public delegate bool MatchConstantDelegate
	(
		ulong address ,
		LinearDisassemblyLine line
	);

	internal static partial class NativeDelegates
	{
		// bool (*matchCallback)(void* matchCtxt, uint64_t addr, BNLinearDisassemblyLine* line)
		public delegate bool MatchConstantDelegate(
			IntPtr matchCtxt,
			ulong address,
			IntPtr line
		);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a public MatchConstantDelegate into the native match-callback shape. The BN core
		// hands the callback an OWNED BNLinearDisassemblyLine* per match (the official Python
		// binding frees it via _LinearDisassemblyLine_convertor -> BNFreeLinearDisassemblyLines).
		// The line is fully copied into managed objects by MustFromNativePointer, so freeing the
		// native line afterward is safe; the earlier form never freed it and leaked one line per
		// match.
		internal sealed class MatchConstantContext
		{
			private readonly MatchConstantDelegate m_callback;

			internal MatchConstantContext(MatchConstantDelegate callback)
			{
				this.m_callback = callback;
			}

			internal bool OnMatch(IntPtr matchCtxt, ulong address, IntPtr line)
			{
				LinearDisassemblyLine copy = LinearDisassemblyLine.MustFromNativePointer(line);

				NativeMethods.BNFreeLinearDisassemblyLines(line, 1);

				return this.m_callback(address, copy);
			}
		}

		internal static NativeDelegates.MatchConstantDelegate WrapMatchConstantDelegate(
			MatchConstantDelegate callback)
		{
			MatchConstantContext context = new MatchConstantContext(callback);

			return new NativeDelegates.MatchConstantDelegate(context.OnMatch);
		}
	}
}
