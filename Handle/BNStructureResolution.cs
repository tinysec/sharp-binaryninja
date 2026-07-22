using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Receives the concrete structure member selected for an access through a structure or base.
	/// </summary>
	public delegate void StructureMemberResolutionCallback(
		NamedTypeReference? baseName,
		Structure? resolvedStructure,
		ulong memberIndex,
		ulong structureOffset,
		ulong adjustedOffset,
		StructureMember member);

	public sealed partial class Structure
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void NativeResolutionCallback(
			IntPtr context,
			IntPtr baseName,
			IntPtr resolvedStructure,
			ulong memberIndex,
			ulong structureOffset,
			ulong adjustedOffset,
			BNStructureMember member);

		private sealed class ResolutionContext
		{
			private readonly StructureMemberResolutionCallback callback;

			private ExceptionDispatchInfo? exception;

			internal ResolutionContext(StructureMemberResolutionCallback callback)
			{
				this.callback = callback;
			}

			internal void Invoke(
				IntPtr context,
				IntPtr baseName,
				IntPtr resolvedStructure,
				ulong memberIndex,
				ulong structureOffset,
				ulong adjustedOffset,
				BNStructureMember member)
			{
				if (null != this.exception)
				{
					return;
				}

				try
				{
					this.callback(
						NamedTypeReference.NewFromHandle(baseName),
						Structure.NewFromHandle(resolvedStructure),
						memberIndex,
						structureOffset,
						adjustedOffset,
						StructureMember.FromNative(member));
				}
				catch (Exception caught)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
				}
			}

			internal void ThrowIfFailed()
			{
				if (null != this.exception)
				{
					this.exception.Throw();
				}
			}
		}

		/// <summary>
		/// Resolves the member selected by an access, following base structures when necessary.
		/// </summary>
		/// <param name="view">Optional binary view used to resolve named base structures.</param>
		/// <param name="offset">Byte offset of the access from the structure start.</param>
		/// <param name="size">Size in bytes of the access.</param>
		/// <param name="resolveCallback">Callback receiving the resolved member and offsets.</param>
		/// <param name="memberIndexHint">Optional preferred member index.</param>
		/// <returns>True when the core resolved a member.</returns>
		public bool ResolveMemberOrBaseMember(
			BinaryView? view,
			ulong offset,
			ulong size,
			StructureMemberResolutionCallback resolveCallback,
			ulong? memberIndexHint = null)
		{
			if (null == resolveCallback)
			{
				throw new ArgumentNullException(nameof(resolveCallback));
			}

			ResolutionContext resolutionContext = new ResolutionContext(resolveCallback);
			NativeResolutionCallback nativeCallback = resolutionContext.Invoke;
			IntPtr nativeCallbackPointer = Marshal.GetFunctionPointerForDelegate(nativeCallback);
			bool result;

			try
			{
				result = NativeMethods.BNResolveStructureMemberOrBaseMember(
					this.handle,
					null == view ? IntPtr.Zero : view.DangerousGetHandle(),
					offset,
					size,
					IntPtr.Zero,
					nativeCallbackPointer,
					memberIndexHint.HasValue,
					memberIndexHint.GetValueOrDefault());
			}
			finally
			{
				GC.KeepAlive(nativeCallback);
			}

			resolutionContext.ThrowIfFailed();

			return result;
		}
	}
}
