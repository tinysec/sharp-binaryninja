using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Provides the native-owned context passed to custom architecture basic-block analysis.
	/// </summary>
	public sealed class BasicBlockAnalysisContext
	{
		private readonly IntPtr handle;

		internal BasicBlockAnalysisContext(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			this.handle = handle;
		}

		internal IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		private BNBasicBlockAnalysisContext Native
		{
			get
			{
				return Marshal.PtrToStructure<BNBasicBlockAnalysisContext>(this.handle);
			}
		}

		/// <summary>
		/// Gets an independently referenced wrapper for the function under analysis.
		/// </summary>
		public Function Function
		{
			get
			{
				return BinaryNinja.Function.MustNewFromHandle(this.Native.function);
			}
		}

		public FunctionAnalysisSkipOverride AnalysisSkipOverride
		{
			get
			{
				return this.Native.analysisSkipOverride;
			}
		}

		public bool GuidedAnalysisMode
		{
			get
			{
				return this.Native.guidedAnalysisMode;
			}
		}

		public bool TriggerGuidedOnInvalidInstruction
		{
			get
			{
				return this.Native.triggerGuidedOnInvalidInstruction;
			}
		}

		public bool TranslateTailCalls
		{
			get
			{
				return this.Native.translateTailCalls;
			}
		}

		public bool DisallowBranchToString
		{
			get
			{
				return this.Native.disallowBranchToString;
			}
		}

		public ulong MaxFunctionSize
		{
			get
			{
				return this.Native.maxFunctionSize;
			}
		}

		public bool MaxSizeReached
		{
			get
			{
				return this.Native.maxSizeReached;
			}
			set
			{
				BNBasicBlockAnalysisContext native = this.Native;
				native.maxSizeReached = value;
				Marshal.StructureToPtr(native, this.handle, false);
			}
		}

		public IndirectBranchInfo[] IndirectBranches
		{
			get
			{
				BNBasicBlockAnalysisContext native = this.Native;
				return UnsafeUtils.ReadStructArray<BNIndirectBranchInfo, IndirectBranchInfo>(
					native.indirectBranches,
					native.indirectBranchesCount,
					IndirectBranchInfo.FromNative);
			}
		}

		public ArchitectureAndAddress[] IndirectNoReturnCalls
		{
			get
			{
				BNBasicBlockAnalysisContext native = this.Native;
				return UnsafeUtils.ReadStructArray<
					BNArchitectureAndAddress,
					ArchitectureAndAddress>(
						native.indirectNoReturnCalls,
						native.indirectNoReturnCallsCount,
						ArchitectureAndAddress.FromNative);
			}
		}

		public IntPtr FunctionArchitectureContext
		{
			get
			{
				return this.Native.functionArchContext;
			}
			set
			{
				BNBasicBlockAnalysisContext native = this.Native;
				if (IntPtr.Zero != native.functionArchContext)
				{
					throw new InvalidOperationException(
						"The function architecture context has already been set.");
				}

				native.functionArchContext = value;
				Marshal.StructureToPtr(native, this.handle, false);
			}
		}

		public BasicBlock? CreateBasicBlock(Architecture architecture, ulong address)
		{
			if (null == architecture)
			{
				throw new ArgumentNullException(nameof(architecture));
			}

			return AnalyzeBasicBlocksContext.CreateBasicBlock(
				this.handle,
				architecture,
				address);
		}

		public void AddBasicBlock(BasicBlock block)
		{
			if (null == block)
			{
				throw new ArgumentNullException(nameof(block));
			}

			AnalyzeBasicBlocksContext.AddBasicBlockToFunction(this.handle, block);
		}

		public void AddTemporaryOutgoingReference(Function target)
		{
			if (null == target)
			{
				throw new ArgumentNullException(nameof(target));
			}

			AnalyzeBasicBlocksContext.AddTempReference(this.handle, target);
		}

		public void SetContextualFunctionReturns(
			ArchitectureAndAddress[] sources,
			bool[] values)
		{
			if (null == sources)
			{
				throw new ArgumentNullException(nameof(sources));
			}

			if (null == values)
			{
				throw new ArgumentNullException(nameof(values));
			}

			if (sources.Length != values.Length)
			{
				throw new ArgumentException(
					"Sources and values must contain the same number of entries.");
			}

			AnalyzeBasicBlocksContext.SetContextualFunctionReturns(
				this.handle,
				sources,
				values);
		}

		public void SetDirectCodeReferences(
			ArchitectureAndAddress[] sources,
			ulong[] targets)
		{
			if (null == sources)
			{
				throw new ArgumentNullException(nameof(sources));
			}

			if (null == targets)
			{
				throw new ArgumentNullException(nameof(targets));
			}

			if (sources.Length != targets.Length)
			{
				throw new ArgumentException(
					"Sources and targets must contain the same number of entries.");
			}

			AnalyzeBasicBlocksContext.SetDirectCodeReferences(
				this.handle,
				sources,
				targets);
		}

		public void SetDirectNoReturnCalls(ArchitectureAndAddress[] sources)
		{
			if (null == sources)
			{
				throw new ArgumentNullException(nameof(sources));
			}

			AnalyzeBasicBlocksContext.SetDirectNoReturnCalls(this.handle, sources);
		}

		public void SetHaltedDisassemblyAddresses(ArchitectureAndAddress[] sources)
		{
			if (null == sources)
			{
				throw new ArgumentNullException(nameof(sources));
			}

			AnalyzeBasicBlocksContext.SetHaltedDisassemblyAddresses(this.handle, sources);
		}

		public void SetInlinedUnresolvedIndirectBranches(
			ArchitectureAndAddress[] sourceAndDestinationPairs)
		{
			if (null == sourceAndDestinationPairs)
			{
				throw new ArgumentNullException(nameof(sourceAndDestinationPairs));
			}

			if (0 != sourceAndDestinationPairs.Length % 2)
			{
				throw new ArgumentException(
					"Source and destination locations must be provided in pairs.",
					nameof(sourceAndDestinationPairs));
			}

			AnalyzeBasicBlocksContext.SetInlinedUnresolvedIndirectBranches(
				this.handle,
				sourceAndDestinationPairs);
		}
	}
}
