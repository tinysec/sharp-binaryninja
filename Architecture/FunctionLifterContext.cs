using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Provides the native-owned context passed to custom architecture function lifting.
	/// </summary>
	public sealed class FunctionLifterContext
	{
		private readonly IntPtr handle;
		private readonly LowLevelILFunction destination;

		internal FunctionLifterContext(
			IntPtr handle,
			LowLevelILFunction destination)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			this.handle = handle;
			this.destination = destination
				?? throw new ArgumentNullException(nameof(destination));
		}

		internal IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		private BNFunctionLifterContext Native
		{
			get
			{
				return Marshal.PtrToStructure<BNFunctionLifterContext>(this.handle);
			}
		}

		/// <summary>
		/// Gets an independently referenced wrapper for the target platform.
		/// </summary>
		public Platform Platform
		{
			get
			{
				return BinaryNinja.Platform.MustNewFromHandle(this.Native.platform);
			}
		}

		/// <summary>
		/// Gets an independently referenced wrapper for the analysis logger.
		/// </summary>
		public Logger Logger
		{
			get
			{
				return BinaryNinja.Logger.MustNewFromHandle(this.Native.logger);
			}
		}

		/// <summary>
		/// Gets independently referenced wrappers for the function's basic blocks.
		/// </summary>
		public BasicBlock[] BasicBlocks
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return UnsafeUtils.ReadHandleArray(
					native.basicBlocks,
					native.basicBlockCount,
					BasicBlock.MustNewFromHandle);
			}
		}

		public ArchitectureAndAddress[] InlinedRemappingKeys
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return this.ReadLocations(
					native.inlinedRemappingKeys,
					native.inlinedRemappingEntryCount);
			}
		}

		public ArchitectureAndAddress[] InlinedRemappingValues
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return this.ReadLocations(
					native.inlinedRemappingValues,
					native.inlinedRemappingEntryCount);
			}
		}

		public IndirectBranchInfo[] IndirectBranches
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return UnsafeUtils.ReadStructArray<BNIndirectBranchInfo, IndirectBranchInfo>(
					native.indirectBranches,
					native.indirectBranchesCount,
					IndirectBranchInfo.FromNative);
			}
		}

		public ArchitectureAndAddress[] NoReturnCalls
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return this.ReadLocations(
					native.noReturnCalls,
					native.noReturnCallsCount);
			}
		}

		public ArchitectureAndAddress[] ContextualFunctionReturnLocations
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return this.ReadLocations(
					native.contextualFunctionReturnLocations,
					native.contextualFunctionReturnCount);
			}
		}

		public bool[] ContextualFunctionReturnValues
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return UnsafeUtils.ReadBoolArray(
					native.contextualFunctionReturnValues,
					native.contextualFunctionReturnCount);
			}
		}

		public ulong[] InlinedCalls
		{
			get
			{
				BNFunctionLifterContext native = this.Native;
				return UnsafeUtils.ReadNumberArray<ulong>(
					native.inlinedCalls,
					native.inlinedCallsCount);
			}
		}

		public IntPtr FunctionArchitectureContext
		{
			get
			{
				return this.Native.functionArchContext;
			}
		}

		public bool ContainsInlinedFunctions
		{
			get
			{
				IntPtr value = this.Native.containsInlinedFunctions;
				return IntPtr.Zero != value && UnsafeUtils.ReadBool(value);
			}
			set
			{
				IntPtr destination = this.Native.containsInlinedFunctions;
				if (IntPtr.Zero == destination)
				{
					throw new InvalidOperationException(
						"The lifter context does not provide an inline-result pointer.");
				}

				Marshal.WriteByte(destination, value ? (byte)1 : (byte)0);
			}
		}

		public void PrepareBlockTranslation(
			LowLevelILFunction function,
			Architecture architecture,
			ulong address)
		{
			if (null == function)
			{
				throw new ArgumentNullException(nameof(function));
			}

			if (null == architecture)
			{
				throw new ArgumentNullException(nameof(architecture));
			}

			NativeMethods.BNPrepareBlockTranslation(
				function.DangerousGetHandle(),
				architecture.DangerousGetHandle(),
				address);
		}

		public unsafe BasicBlock[] PrepareToCopyForeignFunction(
			LowLevelILFunction source)
		{
			if (null == source)
			{
				throw new ArgumentNullException(nameof(source));
			}

			ulong count = 0;
			IntPtr blocks = NativeMethods.BNPrepareToCopyForeignFunction(
				this.destination.DangerousGetHandle(),
				source.DangerousGetHandle(),
				(IntPtr)(&count));
			try
			{
				return UnsafeUtils.ReadHandleArray(
					blocks,
					count,
					BasicBlock.MustNewFromHandle);
			}
			finally
			{
				if (IntPtr.Zero != blocks)
				{
					NativeMethods.BNFreeBasicBlockList(blocks, count);
				}
			}
		}

		public LowLevelILFunction? GetForeignFunctionLiftedIL(Function function)
		{
			if (null == function)
			{
				throw new ArgumentNullException(nameof(function));
			}

			BNFunctionLifterContext native = this.Native;
			ulong[] inlinedCalls = this.InlinedCalls;
			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				IntPtr result = NativeMethods.BNGetForeignFunctionLiftedIL(
					function.DangerousGetHandle(),
					native.logger,
					new UIntPtr((ulong)inlinedCalls.Length),
					allocator.AllocStructArray(inlinedCalls));
				return LowLevelILFunction.TakeHandle(
					result,
					false,
					function.Architecture);
			}
		}

		private ArchitectureAndAddress[] ReadLocations(IntPtr locations, ulong count)
		{
			return UnsafeUtils.ReadStructArray<
				BNArchitectureAndAddress,
				ArchitectureAndAddress>(
					locations,
					count,
					ArchitectureAndAddress.FromNative);
		}
	}
}
