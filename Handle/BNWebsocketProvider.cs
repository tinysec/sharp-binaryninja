using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    /// <summary>Creates WebSocket clients through a registered provider.</summary>
    public abstract class WebsocketProvider :
        AbstractSafeHandle<WebsocketProvider>
    {
        private static readonly object registrationLock = new object();

        private static readonly List<WebsocketProvider> registeredProviders =
            new List<WebsocketProvider>();

        private readonly string registrationName;
        private NativeDelegates.BNWebsocketCreateClient? createClientCallback;

        protected WebsocketProvider(string name)
            : base(IntPtr.Zero, false)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    "A WebSocket provider name is required.",
                    nameof(name)
                );
            }

            this.registrationName = name;
        }

        private protected WebsocketProvider(IntPtr handle)
            : base(handle, false)
        {
            this.registrationName = string.Empty;
        }

        /// <summary>Gets all registered providers.</summary>
        public static IReadOnlyList<WebsocketProvider> GetList()
        {
            IntPtr list = NativeMethods.BNGetWebsocketProviderList(
                out UIntPtr count
            );
            int length = checked((int)count.ToUInt64());
            WebsocketProvider[] providers = new WebsocketProvider[length];
            try
            {
                for (int i = 0; i < length; i++)
                {
                    IntPtr provider = Marshal.ReadIntPtr(
                        list,
                        checked(i * IntPtr.Size)
                    );
                    providers[i] = new CoreWebsocketProvider(provider);
                }
            }
            finally
            {
                NativeMethods.BNFreeWebsocketProviderList(list);
            }

            return providers;
        }

        /// <summary>Gets a registered provider by name.</summary>
        public static WebsocketProvider? GetByName(string name)
        {
            IntPtr provider = NativeMethods.BNGetWebsocketProviderByName(name);
            return IntPtr.Zero == provider
                ? null
                : new CoreWebsocketProvider(provider);
        }

        /// <summary>Gets the provider's registered name.</summary>
        public string Name
        {
            get
            {
                return UnsafeUtils.TakeUtf8String(
                    NativeMethods.BNGetWebsocketProviderName(this.handle)
                );
            }
        }

        /// <summary>Registers this custom provider with the core.</summary>
        public void Register()
        {
            lock (WebsocketProvider.registrationLock)
            {
                if (!this.IsInvalid)
                {
                    throw new InvalidOperationException(
                        "The WebSocket provider is already registered."
                    );
                }

                this.createClientCallback =
                    new NativeDelegates.BNWebsocketCreateClient(
                        this.InvokeCreateClient
                    );
                BNWebsocketProviderCallbacks callbacks =
                    new BNWebsocketProviderCallbacks();
                callbacks.context = IntPtr.Zero;
                callbacks.createClient = Marshal.GetFunctionPointerForDelegate(
                    this.createClientCallback
                );
                IntPtr provider = NativeMethods.BNRegisterWebsocketProvider(
                    this.registrationName,
                    in callbacks
                );
                if (IntPtr.Zero == provider)
                {
                    throw new InvalidOperationException(
                        "The WebSocket provider could not be registered."
                    );
                }

                this.SetHandle(provider);
                WebsocketProvider.registeredProviders.Add(this);
            }
        }

        /// <summary>Creates a client through this provider.</summary>
        public WebsocketClient? CreateClient()
        {
            return WebsocketClient.TakeHandle(
                NativeMethods.BNCreateWebsocketProviderClient(this.handle)
            );
        }

        protected abstract WebsocketClient? CreateNewClient();

        protected override bool ReleaseHandle()
        {
            this.SetHandleAsInvalid();
            return true;
        }

        private IntPtr InvokeCreateClient(IntPtr context)
        {
            try
            {
                WebsocketClient? client = this.CreateNewClient();
                if (null == client || client.IsInvalid)
                {
                    return IntPtr.Zero;
                }

                IntPtr result = NativeMethods.BNNewWebsocketClientReference(
                    client.DangerousGetHandle()
                );
                client.ReleaseFactoryReference();
                return result;
            }
            catch (Exception exception)
            {
                Core.LogError(
                    "Unhandled exception creating a WebSocket client: {0}",
                    exception
                );
                return IntPtr.Zero;
            }
        }

        private sealed class CoreWebsocketProvider : WebsocketProvider
        {
            internal CoreWebsocketProvider(IntPtr handle)
                : base(handle)
            {
            }

            protected override WebsocketClient? CreateNewClient()
            {
                return this.CreateClient();
            }
        }
    }
}
