using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNInlineDuringAnalysisWithConfidence
	{
		/// <summary>
		/// BNInlineDuringAnalysis value
		/// </summary>
		internal InlineDuringAnalysis value;

		/// <summary>
		/// uint8_t confidence
		/// </summary>
		internal byte confidence;
	}

	public sealed class InlineDuringAnalysisWithConfidence
	{
		public InlineDuringAnalysis Value { get; set; } = InlineDuringAnalysis.DoNotInlineCall;

		public byte Confidence { get; set; } = 0;

		public InlineDuringAnalysisWithConfidence()
		{
		}

		public InlineDuringAnalysisWithConfidence(InlineDuringAnalysis value, byte confidence = 0)
		{
			this.Value = value;
			this.Confidence = confidence;
		}

		internal static InlineDuringAnalysisWithConfidence FromNative(
			BNInlineDuringAnalysisWithConfidence native
		)
		{
			return new InlineDuringAnalysisWithConfidence(native.value, native.confidence);
		}

		internal BNInlineDuringAnalysisWithConfidence ToNative()
		{
			return new BNInlineDuringAnalysisWithConfidence()
			{
				value = this.Value,
				confidence = this.Confidence,
			};
		}
	}
}
