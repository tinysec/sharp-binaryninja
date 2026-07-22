using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Stores secrets such as access tokens using an application-defined backend.
	/// </summary>
	public abstract class SecretsProvider : AbstractSafeHandle<SecretsProvider>
	{
		private static readonly object registrationLock = new object();

		private static readonly List<SecretsProvider> registeredProviders =
			new List<SecretsProvider>();

		private readonly string? registrationName;

		private bool isRegistered;

		private NativeDelegates.BNSecretsProviderHasData? hasDataCallback;

		private NativeDelegates.BNSecretsProviderGetData? getDataCallback;

		private NativeDelegates.BNSecretsProviderStoreData? storeDataCallback;

		private NativeDelegates.BNSecretsProviderDeleteData? deleteDataCallback;

		/// <summary>Creates an unregistered custom secrets provider.</summary>
		protected SecretsProvider(string name)
			: base(false)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			this.registrationName = name;
		}

		private SecretsProvider(IntPtr handle)
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
					NativeMethods.BNGetSecretsProviderName(this.handle)
				);
			}
		}

		/// <summary>Registers this provider and roots its callbacks for core use.</summary>
		public void Register()
		{
			if (this.isRegistered || !this.IsInvalid)
			{
				throw new InvalidOperationException("The secrets provider is already registered.");
			}

			this.hasDataCallback = new NativeDelegates.BNSecretsProviderHasData(
				this.InvokeHasData
			);
			this.getDataCallback = new NativeDelegates.BNSecretsProviderGetData(
				this.InvokeGetData
			);
			this.storeDataCallback = new NativeDelegates.BNSecretsProviderStoreData(
				this.InvokeStoreData
			);
			this.deleteDataCallback = new NativeDelegates.BNSecretsProviderDeleteData(
				this.InvokeDeleteData
			);

			BNSecretsProviderCallbacks callbacks = new BNSecretsProviderCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.hasData = Marshal.GetFunctionPointerForDelegate(this.hasDataCallback);
			callbacks.getData = Marshal.GetFunctionPointerForDelegate(this.getDataCallback);
			callbacks.storeData = Marshal.GetFunctionPointerForDelegate(this.storeDataCallback);
			callbacks.deleteData = Marshal.GetFunctionPointerForDelegate(this.deleteDataCallback);

			IntPtr handle = NativeMethods.BNRegisterSecretsProvider(
				this.registrationName ?? string.Empty,
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException("The core rejected the secrets provider.");
			}

			this.SetHandle(handle);
			this.isRegistered = true;
			lock (SecretsProvider.registrationLock)
			{
				SecretsProvider.registeredProviders.Add(this);
			}
		}

		/// <summary>Looks up a registered provider by name.</summary>
		public static SecretsProvider? GetByName(string name)
		{
			if (null == name)
			{
				throw new ArgumentNullException(nameof(name));
			}

			return SecretsProvider.FromHandle(
				NativeMethods.BNGetSecretsProviderByName(name)
			);
		}

		/// <summary>Gets all providers registered with the core.</summary>
		public static SecretsProvider[] GetList()
		{
			IntPtr providers = NativeMethods.BNGetSecretsProviderList(out ulong count);
			return UnsafeUtils.TakeHandleArray<SecretsProvider>(
				providers,
				count,
				SecretsProvider.MustFromHandle,
				NativeMethods.BNFreeSecretsProviderList
			);
		}

		private static SecretsProvider? FromHandle(IntPtr handle)
		{
			if (IntPtr.Zero == handle)
			{
				return null;
			}

			return new CoreSecretsProvider(handle);
		}

		private static SecretsProvider MustFromHandle(IntPtr handle)
		{
			SecretsProvider? provider = SecretsProvider.FromHandle(handle);
			if (null == provider)
			{
				throw new ArgumentNullException(nameof(handle));
			}

			return provider;
		}

		/// <summary>Checks whether data exists for a key without retrieving it.</summary>
		public abstract bool HasData(string key);

		/// <summary>Gets data for a key, or null when the key is absent.</summary>
		public abstract string? GetData(string key);

		/// <summary>Stores data under a key.</summary>
		public abstract bool StoreData(string key, string data);

		/// <summary>Deletes data stored under a key.</summary>
		public abstract bool DeleteData(string key);

		private bool InvokeHasData(IntPtr context, string key)
		{
			try
			{
				return this.HasData(key);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in SecretsProvider.HasData: {0}", exception);
				return false;
			}
		}

		private IntPtr InvokeGetData(IntPtr context, string key)
		{
			try
			{
				string? data = this.GetData(key);
				return null == data ? IntPtr.Zero : NativeMethods.BNAllocString(data);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in SecretsProvider.GetData: {0}", exception);
				return IntPtr.Zero;
			}
		}

		private bool InvokeStoreData(IntPtr context, string key, string data)
		{
			try
			{
				return this.StoreData(key, data);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in SecretsProvider.StoreData: {0}", exception);
				return false;
			}
		}

		private bool InvokeDeleteData(IntPtr context, string key)
		{
			try
			{
				return this.DeleteData(key);
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in SecretsProvider.DeleteData: {0}", exception);
				return false;
			}
		}

		private sealed class CoreSecretsProvider : SecretsProvider
		{
			internal CoreSecretsProvider(IntPtr handle)
				: base(handle)
			{
			}

			public override bool HasData(string key)
			{
				return NativeMethods.BNSecretsProviderHasData(
					this.handle,
					key ?? string.Empty
				);
			}

			public override string? GetData(string key)
			{
				IntPtr data = NativeMethods.BNGetSecretsProviderData(
					this.handle,
					key ?? string.Empty
				);
				if (IntPtr.Zero == data)
				{
					return null;
				}

				return UnsafeUtils.TakeUtf8String(data);
			}

			public override bool StoreData(string key, string data)
			{
				return NativeMethods.BNStoreSecretsProviderData(
					this.handle,
					key ?? string.Empty,
					data ?? string.Empty
				);
			}

			public override bool DeleteData(string key)
			{
				return NativeMethods.BNDeleteSecretsProviderData(
					this.handle,
					key ?? string.Empty
				);
			}
		}
	}
}
