using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class LanguageRepresentationFunction
    {
        private enum CustomStringKind
        {
            CommentStart,
            CommentEnd,
            AnnotationStart,
            AnnotationEnd
        }

        private static readonly object registrationLock = new object();

        private static readonly List<LanguageRepresentationFunction>
            registeredFunctions = new List<LanguageRepresentationFunction>();

        private bool initialReferencePending;

        private NativeDelegates.BNLanguageRepresentationFunctionEvent?
            freeCallback;

        private NativeDelegates.BNLanguageRepresentationTokenEmitter?
            initTokenEmitterCallback;

        private NativeDelegates.BNLanguageRepresentationExpression?
            getExpressionTextCallback;

        private NativeDelegates.BNLanguageRepresentationLines?
            beginLinesCallback;

        private NativeDelegates.BNLanguageRepresentationLines?
            endLinesCallback;

        private NativeDelegates.BNLanguageRepresentationString?
            getCommentStartStringCallback;

        private NativeDelegates.BNLanguageRepresentationString?
            getCommentEndStringCallback;

        private NativeDelegates.BNLanguageRepresentationString?
            getAnnotationStartStringCallback;

        private NativeDelegates.BNLanguageRepresentationString?
            getAnnotationEndStringCallback;

        /// <summary>Creates a custom representation for one high-level IL function.</summary>
        protected LanguageRepresentationFunction(
            LanguageRepresentationFunctionType type,
            Architecture architecture,
            Function owner,
            HighLevelILFunction highLevelIL
        )
            : base(false)
        {
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (null == architecture)
            {
                throw new ArgumentNullException(nameof(architecture));
            }

            if (null == owner)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            if (null == highLevelIL)
            {
                throw new ArgumentNullException(nameof(highLevelIL));
            }

            this.custom = true;
            this.InitializeCustomCallbacks();
            BNCustomLanguageRepresentationFunction callbacks =
                this.CreateCustomCallbacks();
            IntPtr function =
                NativeMethods.BNCreateCustomLanguageRepresentationFunction(
                    type.RegistrationHandle,
                    architecture.DangerousGetHandle(),
                    owner.DangerousGetHandle(),
                    highLevelIL.DangerousGetHandle(),
                    in callbacks
                );
            if (IntPtr.Zero == function)
            {
                throw new InvalidOperationException(
                    "The core rejected the language representation function."
                );
            }

            this.SetHandle(function);
            this.initialReferencePending = true;
            lock (LanguageRepresentationFunction.registrationLock)
            {
                LanguageRepresentationFunction.registeredFunctions.Add(this);
            }
        }

        /// <summary>Initializes a token emitter before rendering.</summary>
        protected virtual void InitTokenEmitter(HighLevelILTokenEmitter emitter)
        {
        }

        /// <summary>Emits tokens for one high-level IL expression.</summary>
        protected abstract void GetExpressionText(
            HighLevelILInstruction instruction,
            HighLevelILTokenEmitter emitter,
            DisassemblySettings? settings,
            OperatorPrecedence precedence,
            bool statement
        );

        /// <summary>Emits tokens before rendering a group of lines.</summary>
        protected virtual void BeginLines(
            HighLevelILInstruction instruction,
            HighLevelILTokenEmitter emitter
        )
        {
        }

        /// <summary>Emits tokens after rendering a group of lines.</summary>
        protected virtual void EndLines(
            HighLevelILInstruction instruction,
            HighLevelILTokenEmitter emitter
        )
        {
        }

        internal void ReleaseInitialReferenceForRegistration()
        {
            if (!this.initialReferencePending)
            {
                return;
            }

            this.initialReferencePending = false;
            NativeMethods.BNFreeLanguageRepresentationFunction(this.handle);
        }

        private void InitializeCustomCallbacks()
        {
            this.freeCallback =
                new NativeDelegates.BNLanguageRepresentationFunctionEvent(
                    this.InvokeFree
                );
            this.initTokenEmitterCallback =
                new NativeDelegates.BNLanguageRepresentationTokenEmitter(
                    this.InvokeInitTokenEmitter
                );
            this.getExpressionTextCallback =
                new NativeDelegates.BNLanguageRepresentationExpression(
                    this.InvokeGetExpressionText
                );
            this.beginLinesCallback =
                new NativeDelegates.BNLanguageRepresentationLines(
                    this.InvokeBeginLines
                );
            this.endLinesCallback =
                new NativeDelegates.BNLanguageRepresentationLines(
                    this.InvokeEndLines
                );
            this.getCommentStartStringCallback =
                new NativeDelegates.BNLanguageRepresentationString(
                    this.InvokeGetCommentStartString
                );
            this.getCommentEndStringCallback =
                new NativeDelegates.BNLanguageRepresentationString(
                    this.InvokeGetCommentEndString
                );
            this.getAnnotationStartStringCallback =
                new NativeDelegates.BNLanguageRepresentationString(
                    this.InvokeGetAnnotationStartString
                );
            this.getAnnotationEndStringCallback =
                new NativeDelegates.BNLanguageRepresentationString(
                    this.InvokeGetAnnotationEndString
                );
        }

        private BNCustomLanguageRepresentationFunction CreateCustomCallbacks()
        {
            BNCustomLanguageRepresentationFunction callbacks =
                new BNCustomLanguageRepresentationFunction();
            callbacks.context = IntPtr.Zero;
            callbacks.freeObject = this.GetCallbackPointer(this.freeCallback!);
            callbacks.externalRefTaken = IntPtr.Zero;
            callbacks.externalRefReleased = IntPtr.Zero;
            callbacks.initTokenEmitter = this.GetCallbackPointer(
                this.initTokenEmitterCallback!
            );
            callbacks.getExprText = this.GetCallbackPointer(
                this.getExpressionTextCallback!
            );
            callbacks.beginLines = this.GetCallbackPointer(
                this.beginLinesCallback!
            );
            callbacks.endLines = this.GetCallbackPointer(
                this.endLinesCallback!
            );
            callbacks.getCommentStartString = this.GetCallbackPointer(
                this.getCommentStartStringCallback!
            );
            callbacks.getCommentEndString = this.GetCallbackPointer(
                this.getCommentEndStringCallback!
            );
            callbacks.getAnnotationStartString = this.GetCallbackPointer(
                this.getAnnotationStartStringCallback!
            );
            callbacks.getAnnotationEndString = this.GetCallbackPointer(
                this.getAnnotationEndStringCallback!
            );

            return callbacks;
        }

        private void InvokeFree(IntPtr context)
        {
            try
            {
                lock (LanguageRepresentationFunction.registrationLock)
                {
                    while (
                        LanguageRepresentationFunction.registeredFunctions.Remove(
                            this
                        )
                    )
                    {
                    }
                }

                this.SetHandleAsInvalid();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("Free", exception);
            }
        }

        private void InvokeInitTokenEmitter(IntPtr context, IntPtr emitter)
        {
            HighLevelILTokenEmitter? managedEmitter = null;
            try
            {
                managedEmitter = HighLevelILTokenEmitter.NewFromHandle(emitter);
                this.InitTokenEmitter(managedEmitter!);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("InitTokenEmitter", exception);
            }
            finally
            {
                if (null != managedEmitter)
                {
                    managedEmitter.Dispose();
                }
            }
        }

        private void InvokeGetExpressionText(
            IntPtr context,
            IntPtr highLevelIL,
            UIntPtr expressionIndex,
            IntPtr emitter,
            IntPtr settings,
            bool asFullAst,
            OperatorPrecedence precedence,
            bool statement
        )
        {
            HighLevelILFunction? managedIL = null;
            HighLevelILTokenEmitter? managedEmitter = null;
            DisassemblySettings? managedSettings = null;
            try
            {
                managedIL = HighLevelILFunction.NewFromHandle(highLevelIL);
                managedEmitter = HighLevelILTokenEmitter.NewFromHandle(emitter);
                managedSettings = DisassemblySettings.NewFromHandle(settings);
                HighLevelILInstruction instruction = managedIL!.MustGetExpression(
                    (HighLevelILExpressionIndex)expressionIndex.ToUInt64(),
                    asFullAst
                );
                this.GetExpressionText(
                    instruction,
                    managedEmitter!,
                    managedSettings,
                    precedence,
                    statement
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetExpressionText", exception);
            }
            finally
            {
                if (null != managedSettings)
                {
                    managedSettings.Dispose();
                }

                if (null != managedEmitter)
                {
                    managedEmitter.Dispose();
                }

                if (null != managedIL)
                {
                    managedIL.Dispose();
                }
            }
        }

        private void InvokeBeginLines(
            IntPtr context,
            IntPtr highLevelIL,
            UIntPtr expressionIndex,
            IntPtr emitter
        )
        {
            this.InvokeLines(
                "BeginLines",
                highLevelIL,
                expressionIndex,
                emitter,
                true
            );
        }

        private void InvokeEndLines(
            IntPtr context,
            IntPtr highLevelIL,
            UIntPtr expressionIndex,
            IntPtr emitter
        )
        {
            this.InvokeLines(
                "EndLines",
                highLevelIL,
                expressionIndex,
                emitter,
                false
            );
        }

        private void InvokeLines(
            string name,
            IntPtr highLevelIL,
            UIntPtr expressionIndex,
            IntPtr emitter,
            bool begin
        )
        {
            HighLevelILFunction? managedIL = null;
            HighLevelILTokenEmitter? managedEmitter = null;
            try
            {
                managedIL = HighLevelILFunction.NewFromHandle(highLevelIL);
                managedEmitter = HighLevelILTokenEmitter.NewFromHandle(emitter);
                HighLevelILInstruction instruction = managedIL!.MustGetExpression(
                    (HighLevelILExpressionIndex)expressionIndex.ToUInt64()
                );
                if (begin)
                {
                    this.BeginLines(instruction, managedEmitter!);
                }
                else
                {
                    this.EndLines(instruction, managedEmitter!);
                }
            }
            catch (Exception exception)
            {
                this.LogCallbackException(name, exception);
            }
            finally
            {
                if (null != managedEmitter)
                {
                    managedEmitter.Dispose();
                }

                if (null != managedIL)
                {
                    managedIL.Dispose();
                }
            }
        }

        private IntPtr InvokeGetCommentStartString(IntPtr context)
        {
            return this.InvokeString(
                "CommentStartString",
                CustomStringKind.CommentStart
            );
        }

        private IntPtr InvokeGetCommentEndString(IntPtr context)
        {
            return this.InvokeString(
                "CommentEndString",
                CustomStringKind.CommentEnd
            );
        }

        private IntPtr InvokeGetAnnotationStartString(IntPtr context)
        {
            return this.InvokeString(
                "AnnotationStartString",
                CustomStringKind.AnnotationStart
            );
        }

        private IntPtr InvokeGetAnnotationEndString(IntPtr context)
        {
            return this.InvokeString(
                "AnnotationEndString",
                CustomStringKind.AnnotationEnd
            );
        }

        private IntPtr InvokeString(string name, CustomStringKind kind)
        {
            try
            {
                string value;
                if (CustomStringKind.CommentStart == kind)
                {
                    value = this.CommentStartString;
                }
                else if (CustomStringKind.CommentEnd == kind)
                {
                    value = this.CommentEndString;
                }
                else if (CustomStringKind.AnnotationStart == kind)
                {
                    value = this.AnnotationStartString;
                }
                else
                {
                    value = this.AnnotationEndString;
                }

                return NativeMethods.BNAllocString(value ?? string.Empty);
            }
            catch (Exception exception)
            {
                this.LogCallbackException(name, exception);

                return NativeMethods.BNAllocString(string.Empty);
            }
        }

        private IntPtr GetCallbackPointer<TDelegate>(TDelegate callback)
            where TDelegate : Delegate
        {
            return Marshal.GetFunctionPointerForDelegate<TDelegate>(callback);
        }

        private void LogCallbackException(string name, Exception exception)
        {
            Core.LogError(
                "Unhandled exception in LanguageRepresentationFunction.{0}: {1}",
                name,
                exception
            );
        }

        private sealed class CoreLanguageRepresentationFunction :
            LanguageRepresentationFunction
        {
            internal CoreLanguageRepresentationFunction(IntPtr handle, bool owner)
                : base(handle, owner)
            {
            }

            protected override void GetExpressionText(
                HighLevelILInstruction instruction,
                HighLevelILTokenEmitter emitter,
                DisassemblySettings? settings,
                OperatorPrecedence precedence,
                bool statement
            )
            {
            }
        }
    }
}
