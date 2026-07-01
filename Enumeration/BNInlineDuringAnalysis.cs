using System;

namespace BinaryNinja
{
	/// <summary>
	/// BNInlineDuringAnalysis
	/// </summary>
    public enum InlineDuringAnalysis : byte
	{
		/// <summary>
		/// The called function should not be inlined.
		/// </summary>
		DoNotInlineCall = 0,

		/// <summary>
		/// The called function should be inlined, preserving original instruction addresses.
		/// </summary>
		InlinePreservingTargetInstructionAddresses = 1,

		/// <summary>
		/// The called function should be inlined, using the call site address for instructions.
		/// </summary>
		InlineUsingCallAddress = 2
	}
}
