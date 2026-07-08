using System;

namespace BinaryNinja
{
	public delegate bool MatchDataDelegate
	(
		ulong address ,
		byte[] data
	);

	internal static partial class NativeDelegates
	{
		// bool (*matchCallback)(void* matchCtxt, uint64_t addr, BNDataBuffer* match)
		public delegate bool MatchDataDelegate(
			IntPtr matchCtxt,
			ulong address,
			IntPtr match
		);
	}

	internal static partial class UnsafeUtils
	{
		// Adapts a public MatchDataDelegate into the native match-callback shape. The BN core hands
		// the callback an OWNED BNDataBuffer per match (the official Python binding adopts it as
		// DataBuffer(handle=match) and frees it in __del__ via BNFreeDataBuffer). The buffer is
		// therefore taken (owned) and freed deterministically with `using`, never borrowed; the
		// earlier borrow form leaked one buffer per match.
		internal sealed class MatchDataContext
		{
			private readonly MatchDataDelegate m_callback;

			internal MatchDataContext(MatchDataDelegate callback)
			{
				this.m_callback = callback;
			}

			internal bool OnMatch(IntPtr matchCtxt, ulong address, IntPtr match)
			{
				using (DataBuffer owned = DataBuffer.MustTakeHandle(match))
				{
					return this.m_callback(address, owned.Contents);
				}
			}
		}

		internal static NativeDelegates.MatchDataDelegate WrapMatchDataDelegate(
			MatchDataDelegate callback)
		{
			MatchDataContext context = new MatchDataContext(callback);

			return new NativeDelegates.MatchDataDelegate(context.OnMatch);
		}
	}
}
