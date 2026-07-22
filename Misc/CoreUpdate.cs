using System;
using System.Runtime.ExceptionServices;

namespace BinaryNinja
{
	/// <summary>Represents an error reported by the Binary Ninja update service.</summary>
	public sealed class UpdateException : Exception
	{
		public UpdateException(string message) : base(message)
		{
		}
	}

	internal static class UpdateInterop
	{
		internal static void ThrowIfError(IntPtr error)
		{
			if (IntPtr.Zero != error)
			{
				throw new UpdateException(UnsafeUtils.TakeUtf8String(error));
			}
		}

	}

	internal sealed class UpdateProgressContext
	{
		private readonly ProgressDelegate? progress;
		private ExceptionDispatchInfo? failure;

		internal UpdateProgressContext(ProgressDelegate? progress)
		{
			this.progress = progress;
		}

		internal bool Invoke(IntPtr context, ulong progress, ulong total)
		{
			try
			{
				if (null == this.progress)
				{
					return true;
				}

				return this.progress(progress, total);
			}
			catch (Exception exception)
			{
				this.failure = ExceptionDispatchInfo.Capture(exception);
				return false;
			}
		}

		internal void ThrowIfFailed()
		{
			if (null != this.failure)
			{
				this.failure.Throw();
			}
		}
	}

	public static partial class Core
	{
		/// <summary>Gets whether automatic updates are enabled.</summary>
		public static bool AreAutoUpdatesEnabled()
		{
			return NativeMethods.BNAreAutoUpdatesEnabled();
		}

		/// <summary>Enables or disables automatic updates.</summary>
		public static void SetAutoUpdatesEnabled(bool enabled)
		{
			NativeMethods.BNSetAutoUpdatesEnabled(enabled);
		}

		/// <summary>Gets the time since the last update check.</summary>
		public static ulong GetTimeSinceLastUpdateCheck()
		{
			return NativeMethods.BNGetTimeSinceLastUpdateCheck();
		}

		/// <summary>Records that the application checked for updates.</summary>
		public static void UpdatesChecked()
		{
			NativeMethods.BNUpdatesChecked();
		}

		/// <summary>Gets or sets the active update channel.</summary>
		public static string ActiveUpdateChannel
		{
			get
			{
				return UnsafeUtils.TakeUtf8String(NativeMethods.BNGetActiveUpdateChannel());
			}
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException(nameof(value));
				}

				NativeMethods.BNSetActiveUpdateChannel(value);
			}
		}

		/// <summary>Installs an update that has already been downloaded.</summary>
		public static void InstallPendingUpdate()
		{
			IntPtr error;
			NativeMethods.BNInstallPendingUpdate(out error);
			UpdateInterop.ThrowIfError(error);
		}
	}
}
