using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class InteractionHandler
    {
        private bool InvokeGetTextLineInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string title
        )
        {
            try
            {
                return WriteStringResult(
                    result,
                    this.GetTextLineInput(prompt, title)
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetTextLineInput", exception);
                return false;
            }
        }

        private bool InvokeGetIntegerInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string title
        )
        {
            try
            {
                long? value = this.GetIntegerInput(prompt, title);
                if (!value.HasValue)
                {
                    return false;
                }

                Marshal.WriteInt64(result, value.Value);
                return true;
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetIntegerInput", exception);
                return false;
            }
        }

        private bool InvokeGetAddressInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string title,
            IntPtr view,
            ulong currentAddress
        )
        {
            BinaryView? managedView = null;
            try
            {
                managedView = NewView(view);
                ulong? value = this.GetAddressInput(
                    prompt,
                    title,
                    managedView,
                    currentAddress
                );
                if (!value.HasValue)
                {
                    return false;
                }

                Marshal.WriteInt64(result, unchecked((long)value.Value));
                return true;
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetAddressInput", exception);
                return false;
            }
            finally
            {
                DisposeView(managedView);
            }
        }

        private bool InvokeGetChoiceInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string title,
            IntPtr choices,
            UIntPtr count
        )
        {
            return this.InvokeChoiceInput(
                false,
                result,
                prompt,
                title,
                choices,
                count
            );
        }

        private bool InvokeGetLargeChoiceInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string title,
            IntPtr choices,
            UIntPtr count
        )
        {
            return this.InvokeChoiceInput(
                true,
                result,
                prompt,
                title,
                choices,
                count
            );
        }

        private bool InvokeChoiceInput(
            bool large,
            IntPtr result,
            string prompt,
            string title,
            IntPtr choices,
            UIntPtr count
        )
        {
            try
            {
                string[] managedChoices = UnsafeUtils.ReadUtf8StringArray(
                    choices,
                    count.ToUInt64()
                );
                ulong? value = large
                    ? this.GetLargeChoiceInput(
                        prompt,
                        title,
                        managedChoices
                    )
                    : this.GetChoiceInput(prompt, title, managedChoices);
                if (!value.HasValue)
                {
                    return false;
                }

                Marshal.WriteInt64(result, unchecked((long)value.Value));
                return true;
            }
            catch (Exception exception)
            {
                this.LogCallbackException(
                    large ? "GetLargeChoiceInput" : "GetChoiceInput",
                    exception
                );
                return false;
            }
        }

        private bool InvokeGetOpenFileNameInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string extension
        )
        {
            try
            {
                return WriteStringResult(
                    result,
                    this.GetOpenFileNameInput(prompt, extension)
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetOpenFileNameInput", exception);
                return false;
            }
        }

        private bool InvokeGetSaveFileNameInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string extension,
            string defaultName
        )
        {
            try
            {
                return WriteStringResult(
                    result,
                    this.GetSaveFileNameInput(
                        prompt,
                        extension,
                        defaultName
                    )
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetSaveFileNameInput", exception);
                return false;
            }
        }

        private bool InvokeGetDirectoryNameInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string defaultName
        )
        {
            try
            {
                return WriteStringResult(
                    result,
                    this.GetDirectoryNameInput(prompt, defaultName)
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetDirectoryNameInput", exception);
                return false;
            }
        }

        private static bool WriteStringResult(
            IntPtr result,
            string? value
        )
        {
            if (null == value)
            {
                return false;
            }

            Marshal.WriteIntPtr(result, NativeMethods.BNAllocString(value));
            return true;
        }

        private bool InvokeGetCheckboxInput(
            IntPtr context,
            IntPtr result,
            string prompt,
            string title,
            IntPtr defaultChoice
        )
        {
            try
            {
                long initial = IntPtr.Zero == defaultChoice
                    ? 0
                    : Marshal.ReadInt64(defaultChoice);
                long? value = this.GetCheckboxInput(prompt, title, initial);
                if (!value.HasValue)
                {
                    return false;
                }

                Marshal.WriteInt64(result, value.Value);
                return true;
            }
            catch (Exception exception)
            {
                this.LogCallbackException("GetCheckboxInput", exception);
                return false;
            }
        }
    }
}
