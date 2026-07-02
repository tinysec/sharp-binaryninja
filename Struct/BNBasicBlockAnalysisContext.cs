using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct BNBasicBlockAnalysisContext 
	{
		/// <summary>
		/// BNFunction* function
		/// </summary>
		public IntPtr function;
		
		/// <summary>
		/// BNFunctionAnalysisSkipOverride analysisSkipOverride
		/// </summary>
		public FunctionAnalysisSkipOverride analysisSkipOverride;
		
		/// <summary>
		/// bool guidedAnalysisMode
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool guidedAnalysisMode;
		
		/// <summary>
		/// bool triggerGuidedOnInvalidInstruction
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool triggerGuidedOnInvalidInstruction;
		
		/// <summary>
		/// bool translateTailCalls
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool translateTailCalls;
		
		/// <summary>
		/// bool disallowBranchToString
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool disallowBranchToString;
		
		/// <summary>
		/// uint64_t maxFunctionSize
		/// </summary>
		public ulong maxFunctionSize;
		
		/// <summary>
		/// uint64_t indirectBranchesCount
		/// </summary>
		public ulong indirectBranchesCount;
		
		/// <summary>
		/// BNIndirectBranchInfo* indirectBranches
		/// </summary>
		public IntPtr indirectBranches;
		
		/// <summary>
		/// uint64_t indirectNoReturnCallsCount
		/// </summary>
		public ulong indirectNoReturnCallsCount;
		
		/// <summary>
		/// BNArchitectureAndAddress* indirectNoReturnCalls
		/// </summary>
		public IntPtr indirectNoReturnCalls;
		
		/// <summary>
		/// bool maxSizeReached
		/// </summary>
		[MarshalAs(UnmanagedType.I1)] public bool maxSizeReached;
		
		/// <summary>
		/// uint64_t contextualFunctionReturnCount
		/// </summary>
		public ulong contextualFunctionReturnCount;
		
		/// <summary>
		/// BNArchitectureAndAddress* contextualFunctionReturnLocations
		/// </summary>
		public IntPtr contextualFunctionReturnLocations;
		
		/// <summary>
		/// bool* contextualFunctionReturnValues
		/// </summary>
		public IntPtr contextualFunctionReturnValues;
		
		/// <summary>
		/// uint64_t directRefCount
		/// </summary>
		public ulong directRefCount;
		
		/// <summary>
		/// BNArchitectureAndAddress* directRefSources
		/// </summary>
		public IntPtr directRefSources;
		
		/// <summary>
		/// uint64_t* directRefTargets
		/// </summary>
		public IntPtr directRefTargets;
		
		/// <summary>
		/// uint64_t directNoReturnCallsCount
		/// </summary>
		public ulong directNoReturnCallsCount;
		
		/// <summary>
		/// BNArchitectureAndAddress* directNoReturnCalls
		/// </summary>
		public IntPtr directNoReturnCalls;
		
		/// <summary>
		/// uint64_t haltedDisassemblyAddressesCount
		/// </summary>
		public ulong haltedDisassemblyAddressesCount;
		
		/// <summary>
		/// BNArchitectureAndAddress* haltedDisassemblyAddresses
		/// </summary>
		public IntPtr haltedDisassemblyAddresses;
		
		/// <summary>
		/// uint64_t inlinedUnresolvedIndirectBranchCount
		/// </summary>
		public ulong inlinedUnresolvedIndirectBranchCount;
		
		/// <summary>
		/// BNArchitectureAndAddress* inlinedUnresolvedIndirectBranches
		/// </summary>
		public IntPtr inlinedUnresolvedIndirectBranches;
	}

    public class BasicBlockAnalysisContext
    {
	    public Function? Function { get; set; } = null;
		
	    public FunctionAnalysisSkipOverride AnalysisSkipOverride { get; set; } =
		    FunctionAnalysisSkipOverride.DefaultFunctionAnalysisSkip;
	    
		public bool GuidedAnalysisMode { get; set; } = false;
		
		public bool TriggerGuidedOnInvalidInstruction { get; set; } = false;
		
		public bool TranslateTailCalls { get; set; } = false;
		
		public bool DisallowBranchToString { get; set; } = false;
	
		public ulong MaxFunctionSize { get; set; } = 0;
	
		public ulong IndirectBranchesCount { get; set; } = 0;
		
		/// <summary>
		/// BNIndirectBranchInfo* indirectBranches
		/// </summary>
		public IntPtr IndirectBranches { get; set; } = IntPtr.Zero;
		
		/// <summary>
		/// uint64_t indirectNoReturnCallsCount
		/// </summary>
		public ulong IndirectNoReturnCallsCount { get; set; } = 0;
		
		/// <summary>
		/// BNArchitectureAndAddress* indirectNoReturnCalls
		/// </summary>
		public IntPtr IndirectNoReturnCalls { get; set; } = IntPtr.Zero;
		
		public bool MaxSizeReached { get; set; } = false;
		
		public ulong ContextualFunctionReturnCount { get; set; } = 0;
		
		/// <summary>
		/// BNArchitectureAndAddress* contextualFunctionReturnLocations
		/// </summary>
		public IntPtr ContextualFunctionReturnLocations { get; set; } = IntPtr.Zero;
		
		/// <summary>
		/// bool* contextualFunctionReturnValues
		/// </summary>
		public IntPtr ContextualFunctionReturnValues { get; set; } = IntPtr.Zero;
		
		/// <summary>
		/// uint64_t directRefCount
		/// </summary>
		public ulong DirectRefCount { get; set; } = 0;
		
		/// <summary>
		/// BNArchitectureAndAddress* directRefSources
		/// </summary>
		public IntPtr DirectRefSources { get; set; } = IntPtr.Zero;
		
		/// <summary>
		/// uint64_t* directRefTargets
		/// </summary>
		public IntPtr DirectRefTargets { get; set; } = IntPtr.Zero;
		
		/// <summary>
		/// uint64_t directNoReturnCallsCount
		/// </summary>
		public ulong DirectNoReturnCallsCount { get; set; } = 0;
		
		/// <summary>
		/// BNArchitectureAndAddress* directNoReturnCalls
		/// </summary>
		public IntPtr DirectNoReturnCalls { get; set; } = IntPtr.Zero;
		
		/// <summary>
		/// uint64_t haltedDisassemblyAddressesCount
		/// </summary>
		public ulong HaltedDisassemblyAddressesCount { get; set; } = 0;
		
		/// <summary>
		/// BNArchitectureAndAddress* haltedDisassemblyAddresses
		/// </summary>
		public IntPtr HaltedDisassemblyAddresses { get; set; } = IntPtr.Zero;
		
		/// <summary>
		/// uint64_t inlinedUnresolvedIndirectBranchCount
		/// </summary>
		public ulong InlinedUnresolvedIndirectBranchCount { get; set; } = 0;
		
		/// <summary>
		/// BNArchitectureAndAddress* inlinedUnresolvedIndirectBranches
		/// </summary>
		public IntPtr InlinedUnresolvedIndirectBranches { get; set; } = IntPtr.Zero;
		
		
		public BasicBlockAnalysisContext() 
		{
		    
		}
    }
}