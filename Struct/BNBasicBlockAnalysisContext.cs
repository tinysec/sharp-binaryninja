using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNBasicBlockAnalysisContext
	{
		internal IntPtr function;
		internal FunctionAnalysisSkipOverride analysisSkipOverride;

		[MarshalAs(UnmanagedType.I1)]
		internal bool guidedAnalysisMode;

		[MarshalAs(UnmanagedType.I1)]
		internal bool triggerGuidedOnInvalidInstruction;

		[MarshalAs(UnmanagedType.I1)]
		internal bool translateTailCalls;

		[MarshalAs(UnmanagedType.I1)]
		internal bool disallowBranchToString;

		internal ulong maxFunctionSize;
		internal ulong indirectBranchesCount;
		internal IntPtr indirectBranches;
		internal ulong indirectNoReturnCallsCount;
		internal IntPtr indirectNoReturnCalls;

		[MarshalAs(UnmanagedType.I1)]
		internal bool maxSizeReached;

		internal ulong contextualFunctionReturnCount;
		internal IntPtr contextualFunctionReturnLocations;
		internal IntPtr contextualFunctionReturnValues;
		internal ulong directRefCount;
		internal IntPtr directRefSources;
		internal IntPtr directRefTargets;
		internal ulong directNoReturnCallsCount;
		internal IntPtr directNoReturnCalls;
		internal ulong haltedDisassemblyAddressesCount;
		internal IntPtr haltedDisassemblyAddresses;
		internal ulong inlinedUnresolvedIndirectBranchCount;
		internal IntPtr inlinedUnresolvedIndirectBranches;
		internal IntPtr functionArchContext;
	}
}
