using System;
using System.Collections.Generic;

namespace BinaryNinja
{
	/// <summary>
	/// Performs transfers for a download provider and bridges request data to the core.
	/// </summary>
	public abstract partial class DownloadInstance : AbstractSafeHandle<DownloadInstance>
	{
		internal DownloadInstance(IntPtr handle, bool owner)
			: base(handle, owner)
		{
		}

		internal static DownloadInstance? NewFromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreDownloadInstance(
				NativeMethods.BNNewDownloadInstanceReference(handle),
				true
			);
		}

		internal static DownloadInstance MustNewFromHandle(IntPtr handle)
		{
			DownloadInstance? instance = DownloadInstance.NewFromHandle(handle);
			if (null == instance)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return instance;
		}

		internal static DownloadInstance? TakeHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreDownloadInstance(handle, true);
		}

		internal static DownloadInstance MustTakeHandle(IntPtr handle)
		{
			DownloadInstance? instance = DownloadInstance.TakeHandle(handle);
			if (null == instance)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return instance;
		}

		internal static DownloadInstance? BorrowHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreDownloadInstance(handle, false);
		}

		internal static DownloadInstance MustBorrowHandle(IntPtr handle)
		{
			DownloadInstance? instance = DownloadInstance.BorrowHandle(handle);
			if (null == instance)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return instance;
		}

		/// <summary>Gets or sets the last error recorded by this instance.</summary>
		public string Error
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetErrorForDownloadInstance(this.handle)
				);
			}

			set
			{
				NativeMethods.BNSetErrorForDownloadInstance(
					this.handle,
					value ?? string.Empty
				);
			}
		}

		/// <summary>Notifies the active request of transfer progress.</summary>
		public bool NotifyProgress(ulong progress, ulong total)
		{
			return NativeMethods.BNNotifyProgressForDownloadInstance(
				this.handle,
				progress,
				total
			);
		}

		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				NativeMethods.BNFreeDownloadInstance(this.handle);
				this.SetHandleAsInvalid();
			}

			return true;
		}

		private sealed class CoreDownloadInstance : DownloadInstance
		{
			internal CoreDownloadInstance(IntPtr handle, bool owner)
				: base(handle, owner)
			{
			}

			protected override int PerformRequest(string url)
			{
				return -1;
			}

			protected override int PerformCustomRequest(
				string method,
				string url,
				IReadOnlyDictionary<string, string> headers,
				out DownloadInstanceResponse? response
			)
			{
				response = null;
				return -1;
			}
		}
	}
}
