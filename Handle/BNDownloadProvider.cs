using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Creates download instances for application-defined transfer implementations.
	/// </summary>
	public abstract class DownloadProvider : AbstractSafeHandle<DownloadProvider>
	{
		private static readonly object registrationLock = new object();

		private static readonly List<DownloadProvider> registeredProviders =
			new List<DownloadProvider>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNDownloadProviderCreateInstance? createInstanceCallback;

		/// <summary>Creates an unregistered custom download provider.</summary>
		protected DownloadProvider(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private DownloadProvider(IntPtr handle)
			: base(handle, false)
		{
		}

		/// <summary>Gets the unique registered provider name.</summary>
		public string Name
		{
			get
			{
				if (this.IsInvalid)
				{
					return this.registrationName ?? string.Empty;
				}

				return UnsafeUtils.TakeUtf8String(
					NativeMethods.BNGetDownloadProviderName(this.handle)
				);
			}
		}

		/// <summary>Registers this provider and roots its callback for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException(
					"The download provider is already registered."
				);
			}

			this.createInstanceCallback =
				new NativeDelegates.BNDownloadProviderCreateInstance(
					this.InvokeCreateInstance
				);
			BNDownloadProviderCallbacks callbacks = new BNDownloadProviderCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.createInstance = Marshal.GetFunctionPointerForDelegate(
				this.createInstanceCallback
			);

			IntPtr handle = NativeMethods.BNRegisterDownloadProvider(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException(
					"The core rejected the download provider."
				);
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (DownloadProvider.registrationLock)
			{
				DownloadProvider.registeredProviders.Add(this);
			}
		}

		/// <summary>Creates the custom instance returned by this provider.</summary>
		public abstract DownloadInstance? CreateNewInstance();

		/// <summary>Looks up a registered download provider by name.</summary>
		public static DownloadProvider? GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return DownloadProvider.FromHandle(
				NativeMethods.BNGetDownloadProviderByName(name)
			);
		}

		/// <summary>Gets every registered download provider.</summary>
		public static unsafe DownloadProvider[] GetList()
		{
			ulong count = 0;
			IntPtr providers = NativeMethods.BNGetDownloadProviderList((IntPtr)(&count));
			return UnsafeUtils.TakeHandleArray<DownloadProvider>(
				providers,
				count,
				DownloadProvider.MustFromHandle,
				NativeMethods.BNFreeDownloadProviderList
			);
		}

		/// <summary>
		/// Creates an owned download instance through this provider's registered callback.
		/// </summary>
		public DownloadInstance? CreateInstance()
		{
			return DownloadInstance.TakeHandle(
				NativeMethods.BNCreateDownloadProviderInstance(this.handle)
			);
		}

		protected override bool ReleaseHandle()
		{
			return true;
		}

		private static DownloadProvider? FromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreDownloadProvider(handle);
		}

		private static DownloadProvider MustFromHandle(IntPtr handle)
		{
			DownloadProvider? provider = DownloadProvider.FromHandle(handle);
			if (null == provider)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return provider;
		}

		private IntPtr InvokeCreateInstance(IntPtr context)
		{
			try
			{
				DownloadInstance? instance = this.CreateNewInstance();
				if (null == instance)
				{
					return IntPtr.Zero;
				}

				IntPtr result = NativeMethods.BNNewDownloadInstanceReference(
					instance.DangerousGetHandle()
				);
				instance.ReleaseInitialReferenceForRegistration();
				return result;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in DownloadProvider.CreateNewInstance: {0}",
					exception
				);
				return IntPtr.Zero;
			}
		}

		private sealed class CoreDownloadProvider : DownloadProvider
		{
			internal CoreDownloadProvider(IntPtr handle)
				: base(handle)
			{
			}

			public override DownloadInstance? CreateNewInstance()
			{
				return this.CreateInstance();
			}
		}
	}
}
