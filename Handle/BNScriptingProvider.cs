using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    /// <summary>
    /// Represents a scripting provider that can create interactive scripting instances.
    /// </summary>
    public abstract class ScriptingProvider : AbstractSafeHandle<ScriptingProvider>
    {
        private static readonly object registrationLock = new object();

        private static readonly List<ScriptingProvider> registeredProviders =
            new List<ScriptingProvider>();

        private readonly string? registrationName;

        private readonly string? registrationApiName;

        private bool isRegistered;

        private NativeDelegates.BNScriptingProviderCreateInstance?
            createInstanceCallback;

        private NativeDelegates.BNScriptingProviderLoadModule? loadModuleCallback;

        private NativeDelegates.BNScriptingProviderInstallModules?
            installModulesCallback;

        internal IntPtr RegistrationHandle
        {
            get
            {
                return this.handle;
            }
        }

        /// <summary>Creates an unregistered custom scripting provider.</summary>
        protected ScriptingProvider(string name, string apiName)
            : base(false)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (null == apiName)
            {
                throw new ArgumentNullException(nameof(apiName));
            }

            this.registrationName = name;
            this.registrationApiName = apiName;
        }

        private ScriptingProvider(IntPtr handle)
            : base(handle, false)
        {
        }

        /// <summary>Gets the human-readable provider name.</summary>
        public string Name
        {
            get
            {
                if (this.IsInvalid)
                {
                    return this.registrationName ?? string.Empty;
                }

                return UnsafeUtils.TakeUtf8String(
                    NativeMethods.BNGetScriptingProviderName(this.handle)
                );
            }
        }

        /// <summary>Gets the programmatic provider API name.</summary>
        public string ApiName
        {
            get
            {
                if (this.IsInvalid)
                {
                    return this.registrationApiName ?? string.Empty;
                }

                return UnsafeUtils.TakeUtf8String(
                    NativeMethods.BNGetScriptingProviderAPIName(this.handle)
                );
            }
        }

        /// <summary>Registers this provider and roots its callbacks for core use.</summary>
        public void Register()
        {
            if (this.isRegistered || !this.IsInvalid)
            {
                throw new InvalidOperationException(
                    "The scripting provider is already registered."
                );
            }

            this.createInstanceCallback =
                new NativeDelegates.BNScriptingProviderCreateInstance(
                    this.InvokeCreateInstance
                );
            this.loadModuleCallback =
                new NativeDelegates.BNScriptingProviderLoadModule(
                    this.InvokeLoadModule
                );
            this.installModulesCallback =
                new NativeDelegates.BNScriptingProviderInstallModules(
                    this.InvokeInstallModules
                );

            BNScriptingProviderCallbacks callbacks =
                new BNScriptingProviderCallbacks();
            callbacks.context = IntPtr.Zero;
            callbacks.createInstance = Marshal.GetFunctionPointerForDelegate(
                this.createInstanceCallback
            );
            callbacks.loadModule = Marshal.GetFunctionPointerForDelegate(
                this.loadModuleCallback
            );
            callbacks.installModules = Marshal.GetFunctionPointerForDelegate(
                this.installModulesCallback
            );

            IntPtr handle = NativeMethods.BNRegisterScriptingProvider(
                this.registrationName ?? string.Empty,
                this.registrationApiName ?? string.Empty,
                in callbacks
            );
            if (IntPtr.Zero == handle)
            {
                throw new InvalidOperationException(
                    "The core rejected the scripting provider."
                );
            }

            this.SetHandle(handle);
            this.isRegistered = true;
            lock (ScriptingProvider.registrationLock)
            {
                ScriptingProvider.registeredProviders.Add(this);
            }
        }

        /// <summary>Creates a custom scripting instance for this provider.</summary>
        public abstract ScriptingInstance? CreateNewInstance();

        /// <summary>Loads a provider module from a repository path.</summary>
        public virtual bool LoadModule(string repository, string module, bool force)
        {
            return false;
        }

        /// <summary>Installs one or more provider modules.</summary>
        public virtual bool InstallModules(string modules)
        {
            return false;
        }

        /// <summary>Gets every registered scripting provider.</summary>
        public static unsafe ScriptingProvider[] GetList()
        {
            ulong count = 0;
            IntPtr providers = NativeMethods.BNGetScriptingProviderList(
                (IntPtr)(&count)
            );
            return UnsafeUtils.TakeHandleArray<ScriptingProvider>(
                providers,
                count,
                ScriptingProvider.MustFromHandle,
                NativeMethods.BNFreeScriptingProviderList
            );
        }

        /// <summary>Looks up a scripting provider by its display name.</summary>
        public static ScriptingProvider? GetByName(string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return ScriptingProvider.FromHandle(
                NativeMethods.BNGetScriptingProviderByName(name)
            );
        }

        /// <summary>Looks up a scripting provider by its programmatic API name.</summary>
        public static ScriptingProvider? GetByApiName(string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return ScriptingProvider.FromHandle(
                NativeMethods.BNGetScriptingProviderByAPIName(name)
            );
        }

        /// <summary>Creates an owned scripting instance through this provider.</summary>
        public ScriptingInstance? CreateInstance()
        {
            return ScriptingInstance.TakeHandle(
                NativeMethods.BNCreateScriptingProviderInstance(this.handle)
            );
        }

        protected override bool ReleaseHandle()
        {
            return true;
        }

        private static ScriptingProvider? FromHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            return new CoreScriptingProvider(handle);
        }

        private static ScriptingProvider MustFromHandle(IntPtr handle)
        {
            ScriptingProvider? provider = ScriptingProvider.FromHandle(handle);
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
                ScriptingInstance? instance = this.CreateNewInstance();
                if (null == instance)
                {
                    return IntPtr.Zero;
                }

                IntPtr result = NativeMethods.BNNewScriptingInstanceReference(
                    instance.DangerousGetHandle()
                );
                instance.ReleaseInitialReferenceForRegistration();
                return result;
            }
            catch (Exception exception)
            {
                Core.LogError(
                    "Unhandled exception in ScriptingProvider.CreateNewInstance: {0}",
                    exception
                );
                return IntPtr.Zero;
            }
        }

        private bool InvokeLoadModule(
            IntPtr context,
            string repository,
            string module,
            bool force
        )
        {
            try
            {
                return this.LoadModule(repository, module, force);
            }
            catch (Exception exception)
            {
                Core.LogError(
                    "Unhandled exception in ScriptingProvider.LoadModule: {0}",
                    exception
                );
                return false;
            }
        }

        private bool InvokeInstallModules(IntPtr context, string modules)
        {
            try
            {
                return this.InstallModules(modules);
            }
            catch (Exception exception)
            {
                Core.LogError(
                    "Unhandled exception in ScriptingProvider.InstallModules: {0}",
                    exception
                );
                return false;
            }
        }

        private sealed class CoreScriptingProvider : ScriptingProvider
        {
            internal CoreScriptingProvider(IntPtr handle)
                : base(handle)
            {
            }

            public override ScriptingInstance? CreateNewInstance()
            {
                return this.CreateInstance();
            }

            public override bool LoadModule(
                string repository,
                string module,
                bool force
            )
            {
                return NativeMethods.BNLoadScriptingProviderModule(
                    this.handle,
                    repository ?? string.Empty,
                    module ?? string.Empty,
                    force
                );
            }

            public override bool InstallModules(string modules)
            {
                return NativeMethods.BNInstallScriptingProviderModules(
                    this.handle,
                    modules ?? string.Empty
                );
            }
        }
    }
}
