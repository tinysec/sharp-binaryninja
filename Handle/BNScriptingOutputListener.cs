using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    /// <summary>Receives output and input-state notifications from a scripting instance.</summary>
    public class ScriptingOutputListener : IDisposable
    {
        private readonly object stateLock = new object();

        private readonly NativeDelegates.BNScriptingOutput outputCallback;

        private readonly NativeDelegates.BNScriptingOutput warningCallback;

        private readonly NativeDelegates.BNScriptingOutput errorCallback;

        private readonly NativeDelegates.BNScriptingInputReadyStateChanged
            inputReadyStateChangedCallback;

        private ScriptingInstance? instance;

        private IntPtr nativeCallbacks;

        private bool disposed;

        /// <summary>Creates an unregistered output listener.</summary>
        public ScriptingOutputListener()
        {
            this.outputCallback = new NativeDelegates.BNScriptingOutput(
                this.InvokeOutput
            );
            this.warningCallback = new NativeDelegates.BNScriptingOutput(
                this.InvokeWarning
            );
            this.errorCallback = new NativeDelegates.BNScriptingOutput(
                this.InvokeError
            );
            this.inputReadyStateChangedCallback =
                new NativeDelegates.BNScriptingInputReadyStateChanged(
                    this.InvokeInputReadyStateChanged
                );
        }

        /// <summary>Handles normal output.</summary>
        public virtual void NotifyOutput(string text)
        {
        }

        /// <summary>Handles warning output.</summary>
        public virtual void NotifyWarning(string text)
        {
        }

        /// <summary>Handles error output.</summary>
        public virtual void NotifyError(string text)
        {
        }

        /// <summary>Handles input-ready state changes.</summary>
        public virtual void NotifyInputReadyStateChanged(
            ScriptingProviderInputReadyState state
        )
        {
        }

        /// <summary>Unregisters this listener and releases its callback block.</summary>
        public void Dispose()
        {
            ScriptingInstance? registeredInstance;
            lock (this.stateLock)
            {
                if (this.disposed)
                {
                    return;
                }

                registeredInstance = this.instance;
            }

            registeredInstance?.UnregisterOutputListener(this);

            lock (this.stateLock)
            {
                this.FreeCallbackBlock();
                this.disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        internal void Register(ScriptingInstance instance)
        {
            lock (this.stateLock)
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException(
                        nameof(ScriptingOutputListener)
                    );
                }

                if (null != this.instance)
                {
                    throw new InvalidOperationException(
                        "The output listener is already registered."
                    );
                }

                BNScriptingOutputListener callbacks =
                    new BNScriptingOutputListener();
                callbacks.context = IntPtr.Zero;
                callbacks.output = Marshal.GetFunctionPointerForDelegate(
                    this.outputCallback
                );
                callbacks.warning = Marshal.GetFunctionPointerForDelegate(
                    this.warningCallback
                );
                callbacks.error = Marshal.GetFunctionPointerForDelegate(
                    this.errorCallback
                );
                callbacks.inputReadyStateChanged =
                    Marshal.GetFunctionPointerForDelegate(
                        this.inputReadyStateChangedCallback
                    );

                this.nativeCallbacks = Marshal.AllocHGlobal(
                    Marshal.SizeOf<BNScriptingOutputListener>()
                );
                Marshal.StructureToPtr(
                    callbacks,
                    this.nativeCallbacks,
                    false
                );
                try
                {
                    NativeMethods.BNRegisterScriptingInstanceOutputListener(
                        instance.DangerousGetHandle(),
                        this.nativeCallbacks
                    );
                    this.instance = instance;
                }
                catch
                {
                    this.FreeCallbackBlock();
                    throw;
                }
            }
        }

        internal void Unregister(ScriptingInstance instance)
        {
            lock (this.stateLock)
            {
                if (!object.ReferenceEquals(this.instance, instance))
                {
                    return;
                }

                NativeMethods.BNUnregisterScriptingInstanceOutputListener(
                    instance.DangerousGetHandle(),
                    this.nativeCallbacks
                );
                this.instance = null;
                this.FreeCallbackBlock();
            }
        }

        private void InvokeOutput(IntPtr context, string text)
        {
            this.InvokeTextNotification("NotifyOutput", this.NotifyOutput, text);
        }

        private void InvokeWarning(IntPtr context, string text)
        {
            this.InvokeTextNotification(
                "NotifyWarning",
                this.NotifyWarning,
                text
            );
        }

        private void InvokeError(IntPtr context, string text)
        {
            this.InvokeTextNotification("NotifyError", this.NotifyError, text);
        }

        private void InvokeInputReadyStateChanged(
            IntPtr context,
            ScriptingProviderInputReadyState state
        )
        {
            try
            {
                this.NotifyInputReadyStateChanged(state);
            }
            catch (Exception exception)
            {
                Core.LogError(
                    "Unhandled exception in ScriptingOutputListener." +
                    "NotifyInputReadyStateChanged: {0}",
                    exception
                );
            }
        }

        private void InvokeTextNotification(
            string name,
            Action<string> notification,
            string text
        )
        {
            try
            {
                notification(text);
            }
            catch (Exception exception)
            {
                Core.LogError(
                    "Unhandled exception in ScriptingOutputListener.{0}: {1}",
                    name,
                    exception
                );
            }
        }

        private void FreeCallbackBlock()
        {
            if (IntPtr.Zero == this.nativeCallbacks)
            {
                return;
            }

            Marshal.FreeHGlobal(this.nativeCallbacks);
            this.nativeCallbacks = IntPtr.Zero;
        }
    }
}
