using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public partial class ScriptingInstance
    {
        private static readonly object registrationLock = new object();

        private static readonly List<ScriptingInstance> registeredInstances =
            new List<ScriptingInstance>();

        private bool initialReferencePending;

        private NativeDelegates.BNScriptingInstanceEvent? destroyCallback;

        private NativeDelegates.BNScriptingInstanceEvent? externalRefTakenCallback;

        private NativeDelegates.BNScriptingInstanceEvent?
            externalRefReleasedCallback;

        private NativeDelegates.BNScriptingInstanceExecuteInput?
            executeInputCallback;

        private NativeDelegates.BNScriptingInstanceExecuteInput?
            executeFileCallback;

        private NativeDelegates.BNScriptingInstanceEvent? cancelCallback;

        private NativeDelegates.BNScriptingInstanceSetObject? releaseViewCallback;

        private NativeDelegates.BNScriptingInstanceSetObject? setViewCallback;

        private NativeDelegates.BNScriptingInstanceSetObject? setFunctionCallback;

        private NativeDelegates.BNScriptingInstanceSetObject? setBlockCallback;

        private NativeDelegates.BNScriptingInstanceSetAddress? setAddressCallback;

        private NativeDelegates.BNScriptingInstanceSetSelection?
            setSelectionCallback;

        private NativeDelegates.BNScriptingInstanceCompleteInput?
            completeInputCallback;

        private NativeDelegates.BNScriptingInstanceEvent? stopCallback;

        /// <summary>Creates a custom instance owned by the supplied provider.</summary>
        protected ScriptingInstance(ScriptingProvider provider)
            : base(false)
        {
            if (null == provider)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.custom = true;
            this.InitializeCallbacks();

            BNScriptingInstanceCallbacks callbacks = this.CreateCallbacks();
            IntPtr handle = NativeMethods.BNInitScriptingInstance(
                provider.RegistrationHandle,
                in callbacks
            );
            if (IntPtr.Zero == handle)
            {
                throw new InvalidOperationException(
                    "The core rejected the scripting instance."
                );
            }

            this.SetHandle(handle);
            this.initialReferencePending = true;
        }

        /// <summary>Releases resources owned by a custom instance.</summary>
        protected virtual void DestroyInstance()
        {
        }

        internal void ReleaseInitialReferenceForRegistration()
        {
            if (!this.initialReferencePending)
            {
                return;
            }

            this.initialReferencePending = false;
            NativeMethods.BNFreeScriptingInstance(this.handle);
        }

        private void InitializeCallbacks()
        {
            this.destroyCallback = new NativeDelegates.BNScriptingInstanceEvent(
                this.InvokeDestroy
            );
            this.externalRefTakenCallback =
                new NativeDelegates.BNScriptingInstanceEvent(
                    this.InvokeExternalReferenceTaken
                );
            this.externalRefReleasedCallback =
                new NativeDelegates.BNScriptingInstanceEvent(
                    this.InvokeExternalReferenceReleased
                );
            this.executeInputCallback =
                new NativeDelegates.BNScriptingInstanceExecuteInput(
                    this.InvokeExecuteInput
                );
            this.executeFileCallback =
                new NativeDelegates.BNScriptingInstanceExecuteInput(
                    this.InvokeExecuteFile
                );
            this.cancelCallback = new NativeDelegates.BNScriptingInstanceEvent(
                this.InvokeCancel
            );
            this.releaseViewCallback =
                new NativeDelegates.BNScriptingInstanceSetObject(
                    this.InvokeReleaseView
                );
            this.setViewCallback =
                new NativeDelegates.BNScriptingInstanceSetObject(
                    this.InvokeSetView
                );
            this.setFunctionCallback =
                new NativeDelegates.BNScriptingInstanceSetObject(
                    this.InvokeSetFunction
                );
            this.setBlockCallback =
                new NativeDelegates.BNScriptingInstanceSetObject(
                    this.InvokeSetBlock
                );
            this.setAddressCallback =
                new NativeDelegates.BNScriptingInstanceSetAddress(
                    this.InvokeSetAddress
                );
            this.setSelectionCallback =
                new NativeDelegates.BNScriptingInstanceSetSelection(
                    this.InvokeSetSelection
                );
            this.completeInputCallback =
                new NativeDelegates.BNScriptingInstanceCompleteInput(
                    this.InvokeCompleteInput
                );
            this.stopCallback = new NativeDelegates.BNScriptingInstanceEvent(
                this.InvokeStop
            );
        }

        private BNScriptingInstanceCallbacks CreateCallbacks()
        {
            BNScriptingInstanceCallbacks callbacks =
                new BNScriptingInstanceCallbacks();
            callbacks.context = IntPtr.Zero;
            callbacks.destroyInstance = Marshal.GetFunctionPointerForDelegate(
                this.destroyCallback!
            );
            callbacks.externalRefTaken = Marshal.GetFunctionPointerForDelegate(
                this.externalRefTakenCallback!
            );
            callbacks.externalRefReleased = Marshal.GetFunctionPointerForDelegate(
                this.externalRefReleasedCallback!
            );
            callbacks.executeScriptInput = Marshal.GetFunctionPointerForDelegate(
                this.executeInputCallback!
            );
            callbacks.executeScriptInputFromFilename =
                Marshal.GetFunctionPointerForDelegate(this.executeFileCallback!);
            callbacks.cancelScriptInput = Marshal.GetFunctionPointerForDelegate(
                this.cancelCallback!
            );
            callbacks.releaseBinaryView = Marshal.GetFunctionPointerForDelegate(
                this.releaseViewCallback!
            );
            callbacks.setCurrentBinaryView = Marshal.GetFunctionPointerForDelegate(
                this.setViewCallback!
            );
            callbacks.setCurrentFunction = Marshal.GetFunctionPointerForDelegate(
                this.setFunctionCallback!
            );
            callbacks.setCurrentBasicBlock = Marshal.GetFunctionPointerForDelegate(
                this.setBlockCallback!
            );
            callbacks.setCurrentAddress = Marshal.GetFunctionPointerForDelegate(
                this.setAddressCallback!
            );
            callbacks.setCurrentSelection = Marshal.GetFunctionPointerForDelegate(
                this.setSelectionCallback!
            );
            callbacks.completeInput = Marshal.GetFunctionPointerForDelegate(
                this.completeInputCallback!
            );
            callbacks.stop = Marshal.GetFunctionPointerForDelegate(
                this.stopCallback!
            );
            return callbacks;
        }

        private void InvokeDestroy(IntPtr context)
        {
            try
            {
                this.DestroyInstance();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("DestroyInstance", exception);
            }
            finally
            {
                lock (ScriptingInstance.registrationLock)
                {
                    while (ScriptingInstance.registeredInstances.Remove(this))
                    {
                    }
                }

                this.SetHandleAsInvalid();
            }
        }

        private void InvokeExternalReferenceTaken(IntPtr context)
        {
            lock (ScriptingInstance.registrationLock)
            {
                ScriptingInstance.registeredInstances.Add(this);
            }
        }

        private void InvokeExternalReferenceReleased(IntPtr context)
        {
            lock (ScriptingInstance.registrationLock)
            {
                ScriptingInstance.registeredInstances.Remove(this);
            }
        }

        private ScriptingProviderExecuteResult InvokeExecuteInput(
            IntPtr context,
            string input
        )
        {
            try
            {
                return this.ExecuteScriptInput(input);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ExecuteScriptInput", exception);
                return ScriptingProviderExecuteResult.InvalidScriptInput;
            }
        }

        private ScriptingProviderExecuteResult InvokeExecuteFile(
            IntPtr context,
            string filename
        )
        {
            try
            {
                return this.ExecuteScriptInputFromFilename(filename);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ExecuteScriptInputFromFile", exception);
                return ScriptingProviderExecuteResult.InvalidScriptInput;
            }
        }

        private void InvokeCancel(IntPtr context)
        {
            this.InvokeAction("CancelScriptInput", this.CancelScriptInput);
        }

        private void InvokeReleaseView(IntPtr context, IntPtr view)
        {
            BinaryView? managedView = this.NewViewReference(view);
            try
            {
                this.ReleaseBinaryView(managedView);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ReleaseBinaryView", exception);
            }
            finally
            {
                managedView?.Dispose();
            }
        }

        private void InvokeSetView(IntPtr context, IntPtr view)
        {
            BinaryView? managedView = this.NewViewReference(view);
            try
            {
                this.SetCurrentBinaryView(managedView);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("SetCurrentBinaryView", exception);
            }
            finally
            {
                managedView?.Dispose();
            }
        }

        private void InvokeSetFunction(IntPtr context, IntPtr function)
        {
            using Function? managedFunction = Function.NewFromHandle(function);
            try
            {
                this.SetCurrentFunction(managedFunction);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("SetCurrentFunction", exception);
            }
        }

        private void InvokeSetBlock(IntPtr context, IntPtr block)
        {
            using BasicBlock? managedBlock = BasicBlock.NewFromHandle(block);
            try
            {
                this.SetCurrentBasicBlock(managedBlock);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("SetCurrentBasicBlock", exception);
            }
        }

        private void InvokeSetAddress(IntPtr context, ulong address)
        {
            try
            {
                this.SetCurrentAddress(address);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("SetCurrentAddress", exception);
            }
        }

        private void InvokeSetSelection(IntPtr context, ulong begin, ulong end)
        {
            try
            {
                this.SetCurrentSelection(begin, end);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("SetCurrentSelection", exception);
            }
        }

        private IntPtr InvokeCompleteInput(
            IntPtr context,
            string text,
            ulong state
        )
        {
            try
            {
                return NativeMethods.BNAllocString(
                    this.CompleteInput(text, state) ?? string.Empty
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("CompleteInput", exception);
                return NativeMethods.BNAllocString(string.Empty);
            }
        }

        private void InvokeStop(IntPtr context)
        {
            this.InvokeAction("Stop", this.Stop);
        }

        private BinaryView? NewViewReference(IntPtr view)
        {
            if (IntPtr.Zero == view)
            {
                return null;
            }

            return BinaryView.TakeHandle(NativeMethods.BNNewViewReference(view));
        }

        private void InvokeAction(string name, Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                this.LogCallbackException(name, exception);
            }
        }

        private void LogCallbackException(string name, Exception exception)
        {
            Core.LogError(
                "Unhandled exception in ScriptingInstance.{0}: {1}",
                name,
                exception
            );
        }
    }
}
