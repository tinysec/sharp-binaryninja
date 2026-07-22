using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	/// <summary>
	/// Applies architecture-specific relocations and describes their native metadata.
	/// </summary>
	public partial class RelocationHandler : AbstractSafeHandle<RelocationHandler>
	{
		/// <summary>Requests automatic coercion for an external relocation pointer.</summary>
		public const ulong AutoCoerceExternPointer = 0xfffffffd;

		private static readonly object registrationLock = new object();

		private static readonly List<RelocationHandler> registeredHandlers =
			new List<RelocationHandler>();

		private readonly bool custom;
		private bool registered;

		internal RelocationHandler(IntPtr handle, bool owner)
			: base(handle, owner)
		{
		}

		/// <summary>Creates a custom relocation handler backed by managed callbacks.</summary>
		protected RelocationHandler()
			: base(true)
		{
			this.custom = true;
			this.InitializeCustomCallbacks();
		}

		internal static RelocationHandler? NewFromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreRelocationHandler(
				NativeMethods.BNNewRelocationHandlerReference(handle), true
			);
		}

		internal static RelocationHandler MustNewFromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new CoreRelocationHandler(
				NativeMethods.BNNewRelocationHandlerReference(handle), true
			);
		}

		internal static RelocationHandler? TakeHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreRelocationHandler(handle, true);
		}

		internal static RelocationHandler MustTakeHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new CoreRelocationHandler(handle, true);
		}

		internal static RelocationHandler? BorrowHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreRelocationHandler(handle, false);
		}

		internal static RelocationHandler MustBorrowHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return new CoreRelocationHandler(handle, false);
		}

		internal static void RootForRegistration(RelocationHandler handler)
		{
			if (null == handler)
			{
				throw new ArgumentNullException(nameof(handler));
			}

			if (!handler.custom)
			{
				return;
			}

			lock (RelocationHandler.registrationLock)
			{
				if (!handler.registered)
				{
					handler.registered = true;
					RelocationHandler.registeredHandlers.Add(handler);
				}
			}
		}

		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid && !this.registered)
			{
				NativeMethods.BNFreeRelocationHandler(this.handle);
				this.SetHandleAsInvalid();
			}

			return true;
		}

		/// <summary>Updates native relocation metadata before relocations are defined.</summary>
		/// <param name="view">The binary view containing the relocations.</param>
		/// <param name="architecture">The architecture interpreting the relocations.</param>
		/// <param name="result">The metadata records to inspect and update in place.</param>
		/// <returns>True when the metadata was recognized and updated.</returns>
		public virtual bool GetRelocationInfo(
			BinaryView view,
			Architecture architecture,
			RelocationInfo[] result
		)
		{
			return false;
		}

		/// <summary>Applies one relocation to a destination buffer.</summary>
		public virtual bool ApplyRelocation(
			BinaryView view,
			Architecture architecture,
			Relocation relocation,
			IntPtr destination,
			ulong length
		)
		{
			return this.DefaultApplyRelocation(
				view, architecture, relocation, destination, length
			);
		}

		/// <summary>Applies one relocation using the core default implementation.</summary>
		public bool DefaultApplyRelocation(
			BinaryView view,
			Architecture architecture,
			Relocation relocation,
			IntPtr destination,
			ulong length
		)
		{
			RelocationHandler.ValidateRelocationArguments(
				view, architecture, relocation
			);
			return NativeMethods.BNRelocationHandlerDefaultApplyRelocation(
				this.handle,
				view.DangerousGetHandle(),
				architecture.DangerousGetHandle(),
				relocation.DangerousGetHandle(),
				destination,
				length
			);
		}

		/// <summary>Gets the LLIL operand for an external relocation.</summary>
		public virtual ulong GetOperandForExternalRelocation(
			IntPtr data,
			ulong address,
			ulong length,
			LowLevelILFunction lowLevelIl,
			Relocation relocation
		)
		{
			return RelocationHandler.AutoCoerceExternPointer;
		}

		private static void ValidateRelocationArguments(
			BinaryView view,
			Architecture architecture,
			Relocation relocation
		)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			if (null == architecture)
			{
				throw new ArgumentNullException(nameof(architecture));
			}

			if (null == relocation)
			{
				throw new ArgumentNullException(nameof(relocation));
			}
		}

		private static void ValidateRelocationInfoArguments(
			BinaryView view,
			Architecture architecture,
			RelocationInfo[] result
		)
		{
			if (null == view)
			{
				throw new ArgumentNullException(nameof(view));
			}

			if (null == architecture)
			{
				throw new ArgumentNullException(nameof(architecture));
			}

			if (null == result)
			{
				throw new ArgumentNullException(nameof(result));
			}

			foreach (RelocationInfo info in result)
			{
				if (null == info)
				{
					throw new ArgumentException(
						"Relocation metadata cannot contain null values.",
						nameof(result)
					);
				}
			}
		}

		private static BNRelocationInfo[] CreateNativeRelocationInfo(
			RelocationInfo[] result
		)
		{
			BNRelocationInfo[] nativeResult = new BNRelocationInfo[result.Length];
			for (int i = 0; i < result.Length; i++)
			{
				nativeResult[i] = result[i].ToNative(default(BNRelocationInfo));
			}

			return nativeResult;
		}
	}
}
