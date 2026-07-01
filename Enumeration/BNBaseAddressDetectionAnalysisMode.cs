using System;

namespace BinaryNinja
{
	/// <summary>
	/// BNBaseAddressDetectionAnalysisMode
	/// </summary>
    public enum BaseAddressDetectionAnalysisMode : byte
	{
		InstructionAnalysisBaseAddressDetection = 0,
		SamplingBaseAddressDetection = 1
	}
}
