using System;

namespace BinaryNinja
{
    /// <summary>Represents one interactive instance of a scripting provider.</summary>
    public partial class ScriptingInstance :
        AbstractSafeHandle<ScriptingInstance>
    {
        private bool custom;

        public ScriptingInstance(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        internal static ScriptingInstance? NewFromHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            return new ScriptingInstance(
                NativeMethods.BNNewScriptingInstanceReference(handle),
                true
            );
        }

        internal static ScriptingInstance MustNewFromHandle(IntPtr handle)
        {
            ScriptingInstance? instance = ScriptingInstance.NewFromHandle(handle);
            if (null == instance)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return instance;
        }

        internal static ScriptingInstance? TakeHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            return new ScriptingInstance(handle, true);
        }

        internal static ScriptingInstance MustTakeHandle(IntPtr handle)
        {
            ScriptingInstance? instance = ScriptingInstance.TakeHandle(handle);
            if (null == instance)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return instance;
        }

        internal static ScriptingInstance? BorrowHandle(IntPtr handle)
        {
            if (IntPtr.Zero == handle)
            {
                return null;
            }

            return new ScriptingInstance(handle, false);
        }

        internal static ScriptingInstance MustBorrowHandle(IntPtr handle)
        {
            ScriptingInstance? instance = ScriptingInstance.BorrowHandle(handle);
            if (null == instance)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            return instance;
        }

        /// <summary>Gets or notifies the current input-ready state.</summary>
        public ScriptingProviderInputReadyState InputReadyState
        {
            get
            {
                return NativeMethods.BNGetScriptingInstanceInputReadyState(
                    this.handle
                );
            }

            set
            {
                this.NotifyInputReadyStateChanged(value);
            }
        }

        /// <summary>Gets or sets the token delimiters used for input completion.</summary>
        public string Delimiters
        {
            get
            {
                return UnsafeUtils.TakeUtf8String(
                    NativeMethods.BNGetScriptingInstanceDelimiters(this.handle)
                );
            }

            set
            {
                NativeMethods.BNSetScriptingInstanceDelimiters(
                    this.handle,
                    value ?? string.Empty
                );
            }
        }

        /// <summary>Executes script text.</summary>
        public virtual ScriptingProviderExecuteResult ExecuteScriptInput(
            string input
        )
        {
            if (this.custom)
            {
                return ScriptingProviderExecuteResult.InvalidScriptInput;
            }

            return NativeMethods.BNExecuteScriptInput(
                this.handle,
                input ?? string.Empty
            );
        }

        /// <summary>Executes script text read from a file.</summary>
        public virtual ScriptingProviderExecuteResult ExecuteScriptInputFromFile(
            string filename
        )
        {
            if (this.custom)
            {
                return ScriptingProviderExecuteResult.InvalidScriptInput;
            }

            return NativeMethods.BNExecuteScriptInputFromFilename(
                this.handle,
                filename ?? string.Empty
            );
        }

        /// <summary>Executes script text read from a file.</summary>
        public virtual ScriptingProviderExecuteResult
            ExecuteScriptInputFromFilename(string filename)
        {
            return this.ExecuteScriptInputFromFile(filename);
        }

        /// <summary>Cancels the active script input.</summary>
        public virtual void CancelScriptInput()
        {
            if (!this.custom)
            {
                NativeMethods.BNCancelScriptInput(this.handle);
            }
        }

        /// <summary>Releases a view previously supplied to this instance.</summary>
        public virtual void ReleaseBinaryView(BinaryView? view)
        {
            if (!this.custom)
            {
                NativeMethods.BNScriptingInstanceReleaseBinaryView(
                    this.handle,
                    null == view ? IntPtr.Zero : view.DangerousGetHandle()
                );
            }
        }

        /// <summary>Sets the current binary view.</summary>
        public virtual void SetCurrentBinaryView(BinaryView? view)
        {
            if (!this.custom)
            {
                NativeMethods.BNSetScriptingInstanceCurrentBinaryView(
                    this.handle,
                    null == view ? IntPtr.Zero : view.DangerousGetHandle()
                );
            }
        }

        /// <summary>Sets the current function.</summary>
        public virtual void SetCurrentFunction(Function? function)
        {
            if (!this.custom)
            {
                NativeMethods.BNSetScriptingInstanceCurrentFunction(
                    this.handle,
                    null == function ? IntPtr.Zero : function.DangerousGetHandle()
                );
            }
        }

        /// <summary>Sets the current basic block.</summary>
        public virtual void SetCurrentBasicBlock(BasicBlock? block)
        {
            if (!this.custom)
            {
                NativeMethods.BNSetScriptingInstanceCurrentBasicBlock(
                    this.handle,
                    null == block ? IntPtr.Zero : block.DangerousGetHandle()
                );
            }
        }

        /// <summary>Sets the current address.</summary>
        public virtual void SetCurrentAddress(ulong address)
        {
            if (!this.custom)
            {
                NativeMethods.BNSetScriptingInstanceCurrentAddress(
                    this.handle,
                    address
                );
            }
        }

        /// <summary>Sets the current selection.</summary>
        public virtual void SetCurrentSelection(ulong begin, ulong end)
        {
            if (!this.custom)
            {
                NativeMethods.BNSetScriptingInstanceCurrentSelection(
                    this.handle,
                    begin,
                    end
                );
            }
        }

        /// <summary>Completes input text at the supplied completion state.</summary>
        public virtual string CompleteInput(string text, ulong state)
        {
            if (this.custom)
            {
                return string.Empty;
            }

            return UnsafeUtils.TakeUtf8String(
                NativeMethods.BNScriptingInstanceCompleteInput(
                    this.handle,
                    text ?? string.Empty,
                    state
                )
            );
        }

        /// <summary>Stops this scripting instance.</summary>
        public virtual void Stop()
        {
            if (!this.custom)
            {
                NativeMethods.BNStopScriptingInstance(this.handle);
            }
        }

        /// <summary>Emits normal output to registered listeners.</summary>
        public void Output(string text)
        {
            NativeMethods.BNNotifyOutputForScriptingInstance(
                this.handle,
                text ?? string.Empty
            );
        }

        /// <summary>Emits warning output to registered listeners.</summary>
        public void Warning(string text)
        {
            NativeMethods.BNNotifyWarningForScriptingInstance(
                this.handle,
                text ?? string.Empty
            );
        }

        /// <summary>Emits error output to registered listeners.</summary>
        public void Error(string text)
        {
            NativeMethods.BNNotifyErrorForScriptingInstance(
                this.handle,
                text ?? string.Empty
            );
        }

        /// <summary>Notifies listeners that the input-ready state changed.</summary>
        public void NotifyInputReadyStateChanged(
            ScriptingProviderInputReadyState state
        )
        {
            NativeMethods.BNNotifyInputReadyStateForScriptingInstance(
                this.handle,
                state
            );
        }

        protected override bool ReleaseHandle()
        {
            if (this.custom)
            {
                if (this.initialReferencePending)
                {
                    this.initialReferencePending = false;
                    NativeMethods.BNFreeScriptingInstance(this.handle);
                }

                return true;
            }

            if (!this.IsInvalid)
            {
                this.UnregisterAllOutputListeners();
                NativeMethods.BNFreeScriptingInstance(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

    }
}
