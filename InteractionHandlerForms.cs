using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class InteractionHandler
    {
        private bool InvokeGetFormInput(
            IntPtr context,
            IntPtr fields,
            UIntPtr count,
            string title
        )
        {
            FormInputField[]? managedFields = null;
            IntPtr[]? stringResults = null;
            int length = 0;
            try
            {
                length = checked((int)count.ToUInt64());
                managedFields = new FormInputField[length];
                stringResults = new IntPtr[length];
                int size = Marshal.SizeOf<BNFormInputField>();
                for (int i = 0; i < length; i++)
                {
                    IntPtr fieldPointer = IntPtr.Add(fields, checked(i * size));
                    BNFormInputField native =
                        Marshal.PtrToStructure<BNFormInputField>(fieldPointer);
                    managedFields[i] = FormInputField.FromNative(native);
                }

                if (!this.GetFormInput(managedFields, title))
                {
                    return false;
                }

                for (int i = 0; i < length; i++)
                {
                    IntPtr fieldPointer = IntPtr.Add(fields, checked(i * size));
                    BNFormInputField native =
                        Marshal.PtrToStructure<BNFormInputField>(fieldPointer);
                    stringResults[i] = managedFields[i].WriteResult(ref native);
                    Marshal.StructureToPtr(native, fieldPointer, false);
                }

                return true;
            }
            catch (Exception exception)
            {
                try
                {
                    ReleaseStringResults(stringResults);
                }
                catch (Exception cleanupException)
                {
                    this.LogCallbackException(
                        "ReleaseFormResults",
                        cleanupException
                    );
                }

                this.LogCallbackException("GetFormInput", exception);
                return false;
            }
            finally
            {
                if (null != managedFields)
                {
                    foreach (FormInputField field in managedFields)
                    {
                        if (null != field)
                        {
                            try
                            {
                                field.ReleaseCallbackReferences();
                            }
                            catch (Exception exception)
                            {
                                this.LogCallbackException(
                                    "ReleaseFormReferences",
                                    exception
                                );
                            }
                        }
                    }
                }
            }
        }

        private static void ReleaseStringResults(IntPtr[]? stringResults)
        {
            if (null == stringResults)
            {
                return;
            }

            for (int i = 0; i < stringResults.Length; i++)
            {
                if (IntPtr.Zero == stringResults[i])
                {
                    continue;
                }

                NativeMethods.BNFreeString(stringResults[i]);
            }
        }

        private MessageBoxButtonResult InvokeShowMessageBox(
            IntPtr context,
            string title,
            string text,
            MessageBoxButtonSet buttons,
            MessageBoxIcon icon
        )
        {
            try
            {
                return this.ShowMessageBox(title, text, buttons, icon);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ShowMessageBox", exception);
                return MessageBoxButtonResult.CancelButton;
            }
        }

        private bool InvokeOpenUrl(IntPtr context, string url)
        {
            try
            {
                return this.OpenUrl(url);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("OpenUrl", exception);
                return false;
            }
        }

        private bool InvokeRunProgressDialog(
            IntPtr context,
            string title,
            bool canCancel,
            IntPtr task,
            IntPtr taskContext
        )
        {
            try
            {
                InteractionProgressTask managedTask =
                    new InteractionProgressTask(task, taskContext);
                return this.RunProgressDialog(
                    title,
                    canCancel,
                    managedTask.Invoke
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("RunProgressDialog", exception);
                return false;
            }
        }

        private sealed class InteractionProgressTask
        {
            private readonly NativeDelegates.BNInteractionProgressTask task;
            private readonly IntPtr taskContext;
            private ProgressDelegate? progress;

            internal InteractionProgressTask(IntPtr task, IntPtr taskContext)
            {
                this.task = Marshal.GetDelegateForFunctionPointer<
                    NativeDelegates.BNInteractionProgressTask
                >(task);
                this.taskContext = taskContext;
            }

            internal void Invoke(ProgressDelegate progress)
            {
                this.progress = progress;
                NativeDelegates.BNProgressFunction nativeProgress =
                    new NativeDelegates.BNProgressFunction(this.InvokeProgress);
                try
                {
                    this.task(
                        this.taskContext,
                        Marshal.GetFunctionPointerForDelegate(nativeProgress),
                        IntPtr.Zero
                    );
                }
                finally
                {
                    GC.KeepAlive(nativeProgress);
                    this.progress = null;
                }
            }

            private bool InvokeProgress(
                IntPtr context,
                ulong current,
                ulong total
            )
            {
                ProgressDelegate? currentProgress = this.progress;
                if (null == currentProgress)
                {
                    return false;
                }

                try
                {
                    return currentProgress(current, total);
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
