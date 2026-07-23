using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct BNFunctionLifterContext
	{
		internal IntPtr platform;
		internal IntPtr logger;
		internal ulong basicBlockCount;
		internal IntPtr basicBlocks;
		internal ulong inlinedRemappingEntryCount;
		internal IntPtr inlinedRemappingKeys;
		internal IntPtr inlinedRemappingValues;
		internal ulong indirectBranchesCount;
		internal IntPtr indirectBranches;
		internal ulong noReturnCallsCount;
		internal IntPtr noReturnCalls;
		internal ulong contextualFunctionReturnCount;
		internal IntPtr contextualFunctionReturnLocations;
		internal IntPtr contextualFunctionReturnValues;
		internal ulong inlinedCallsCount;
		internal IntPtr inlinedCalls;
		internal IntPtr functionArchContext;
		internal IntPtr containsInlinedFunctions;
	}
}
