using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct BNAnalysisParameters 
	{
		/// <summary>
		/// uint64_t maxAnalysisTime
		/// </summary>
		internal ulong maxAnalysisTime;
		
		/// <summary>
		/// uint64_t maxFunctionSize
		/// </summary>
		internal ulong maxFunctionSize;
		
		/// <summary>
		/// uint64_t maxFunctionAnalysisTime
		/// </summary>
		internal ulong maxFunctionAnalysisTime;
		
		/// <summary>
		/// uint64_t maxFunctionUpdateCount
		/// </summary>
		internal ulong maxFunctionUpdateCount;
		
		/// <summary>
		/// uint64_t maxFunctionSubmitCount
		/// </summary>
		internal ulong maxFunctionSubmitCount;
		
		/// <summary>
		/// bool suppressNewAutoFunctionAnalysis
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool suppressNewAutoFunctionAnalysis;
		
		/// <summary>
		/// BNAnalysisMode mode
		/// </summary>
		internal AnalysisMode mode;
		
		/// <summary>
		/// bool alwaysAnalyzeIndirectBranches
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] internal bool alwaysAnalyzeIndirectBranches;
		
		/// <summary>
		/// uint64_t advancedAnalysisCacheSize
		/// </summary>
		internal ulong advancedAnalysisCacheSize;
	}

    public sealed class AnalysisParameters : INativeWrapper<BNAnalysisParameters>
    {
		public ulong MaxAnalysisTime { get; set; } = 0;
		
		public ulong MaxFunctionSize { get; set; } = 0;
		
		public ulong MaxFunctionAnalysisTime { get; set; } = 0;
		
		public ulong MaxFunctionUpdateCount { get; set; } = 0;
		
		public ulong MaxFunctionSubmitCount { get; set; } = 0;
		
		public bool SuppressNewAutoFunctionAnalysis { get; set; } = false;

		public AnalysisMode Mode { get; set; } = AnalysisMode.FullAnalysisMode;
	
		public bool AlwaysAnalyzeIndirectBranches { get; set; } = false;
		
		public ulong AdvancedAnalysisCacheSize { get; set; } = 0;
		
		public AnalysisParameters() 
		{
		    
		}

		internal static AnalysisParameters FromNative(BNAnalysisParameters native)
		{
			return new AnalysisParameters()
			{
				MaxAnalysisTime = native.maxAnalysisTime ,
				MaxFunctionSize = native.maxFunctionSize ,
				MaxFunctionAnalysisTime = native.maxFunctionAnalysisTime ,
				MaxFunctionUpdateCount = native.maxFunctionUpdateCount ,
				MaxFunctionSubmitCount = native.maxFunctionSubmitCount ,
				SuppressNewAutoFunctionAnalysis = native.suppressNewAutoFunctionAnalysis ,
				Mode = native.mode ,
				AlwaysAnalyzeIndirectBranches = native.alwaysAnalyzeIndirectBranches ,
				AdvancedAnalysisCacheSize = native.advancedAnalysisCacheSize
			};
		}

		public BNAnalysisParameters ToNative()
		{
			return new BNAnalysisParameters()
			{
				maxAnalysisTime = this.MaxAnalysisTime ,
				maxFunctionSize = this.MaxFunctionSize ,
				maxFunctionAnalysisTime = this.MaxFunctionAnalysisTime ,
				maxFunctionUpdateCount = this.MaxFunctionUpdateCount ,
				maxFunctionSubmitCount = this.MaxFunctionSubmitCount ,
				suppressNewAutoFunctionAnalysis = this.SuppressNewAutoFunctionAnalysis ,
				mode = this.Mode ,
				alwaysAnalyzeIndirectBranches = this.AlwaysAnalyzeIndirectBranches ,
				advancedAnalysisCacheSize = this.AdvancedAnalysisCacheSize
			};
		}
    }
}