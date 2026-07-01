using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNBaseAddressDetectionSettings 
	{
		/// <summary>
		/// const char* Architecture
		/// </summary>
		public IntPtr Architecture;
		
		/// <summary>
		/// const char* Analysis
		/// </summary>
		public IntPtr Analysis;
		
		/// <summary>
		/// uint32_t MinStrlen
		/// </summary>
		public uint MinStrlen;
		
		/// <summary>
		/// uint32_t Alignment
		/// </summary>
		public uint Alignment;
		
		/// <summary>
		/// uint64_t LowerBoundary
		/// </summary>
		public ulong LowerBoundary;
		
		/// <summary>
		/// uint64_t UpperBoundary
		/// </summary>
		public ulong UpperBoundary;
		
		/// <summary>
		/// BNBaseAddressDetectionPOISetting POIAnalysis
		/// </summary>
		public BaseAddressDetectionPOISetting POIAnalysis;
		
		/// <summary>
		/// uint32_t MaxPointersPerCluster
		/// </summary>
		public uint MaxPointersPerCluster;

		/// <summary>
		/// BNBaseAddressDetectionAnalysisMode AnalysisMode
		/// </summary>
		public BaseAddressDetectionAnalysisMode AnalysisMode;
	}

    public sealed class BaseAddressDetectionSettings 
    {
		public string Architecture { get; set; } = string.Empty;
		
		public string Analysis { get; set; } = string.Empty;
		
		public uint MinStrlen { get; set; } = 0;
	
		public uint Alignment { get; set; } = 0;
	
		public ulong LowerBoundary { get; set; } = 0;
	
		public ulong UpperBoundary { get; set; } = 0;

		public BaseAddressDetectionPOISetting POIAnalysis { get; set; } =
			BaseAddressDetectionPOISetting.POIAnalysisStringsOnly;
		
		public uint MaxPointersPerCluster { get; set; } = 0;

		public BaseAddressDetectionAnalysisMode AnalysisMode { get; set; } =
			BaseAddressDetectionAnalysisMode.InstructionAnalysisBaseAddressDetection;

		public BaseAddressDetectionSettings()
		{
		    
		}
    }
}