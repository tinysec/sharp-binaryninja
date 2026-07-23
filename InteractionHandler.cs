using System;

namespace BinaryNinja
{
    /// <summary>Handles reports, prompts, and progress requests from the core.</summary>
    public abstract partial class InteractionHandler
    {
        private static readonly object registrationLock = new object();

        private static InteractionHandler? registeredHandler;

        /// <summary>Registers this handler as the process-wide interaction handler.</summary>
        public void Register()
        {
            lock (InteractionHandler.registrationLock)
            {
                this.InitializeCallbacks();
                BNInteractionHandlerCallbacks callbacks = this.CreateCallbacks();
                NativeMethods.BNRegisterInteractionHandler(in callbacks);
                InteractionHandler.registeredHandler = this;
            }
        }

        public abstract void ShowPlainTextReport(
            BinaryView? view,
            string title,
            string contents
        );

        public virtual void ShowMarkdownReport(
            BinaryView? view,
            string title,
            string contents,
            string plainText
        )
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                this.ShowPlainTextReport(view, title, plainText);
            }
        }

        public virtual void ShowHTMLReport(
            BinaryView? view,
            string title,
            string contents,
            string plainText
        )
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                this.ShowPlainTextReport(view, title, plainText);
            }
        }

        public virtual void ShowGraphReport(
            BinaryView? view,
            string title,
            FlowGraph graph
        )
        {
        }

        public virtual void ShowReportCollection(
            string title,
            ReportCollection reports
        )
        {
        }

        public abstract string? GetTextLineInput(string prompt, string title);

        public virtual long? GetIntegerInput(string prompt, string title)
        {
            while (true)
            {
                string? text = this.GetTextLineInput(prompt, title);
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                if (long.TryParse(text, out long value))
                {
                    return value;
                }
            }
        }

        public virtual ulong? GetAddressInput(
            string prompt,
            string title,
            BinaryView? view,
            ulong currentAddress
        )
        {
            long? value = this.GetIntegerInput(prompt, title);
            return value.HasValue ? unchecked((ulong)value.Value) : null;
        }

        public abstract ulong? GetChoiceInput(
            string prompt,
            string title,
            string[] choices
        );

        public abstract ulong? GetLargeChoiceInput(
            string prompt,
            string title,
            string[] choices
        );

        public virtual string? GetOpenFileNameInput(
            string prompt,
            string extension
        )
        {
            return this.GetTextLineInput(prompt, "Open File");
        }

        public virtual string? GetSaveFileNameInput(
            string prompt,
            string extension,
            string defaultName
        )
        {
            return this.GetTextLineInput(prompt, "Save File");
        }

        public virtual string? GetDirectoryNameInput(
            string prompt,
            string defaultName
        )
        {
            return this.GetTextLineInput(prompt, "Select Directory");
        }

        public virtual long? GetCheckboxInput(
            string prompt,
            string title,
            long defaultChoice
        )
        {
            return null;
        }

        public abstract bool GetFormInput(FormInputField[] fields, string title);

        public abstract MessageBoxButtonResult ShowMessageBox(
            string title,
            string text,
            MessageBoxButtonSet buttons,
            MessageBoxIcon icon
        );

        public abstract bool OpenUrl(string url);

        public virtual bool RunProgressDialog(
            string title,
            bool canCancel,
            ProgressTaskDelegate task
        )
        {
            task(InteractionHandler.ContinueProgress);

            return true;
        }

        private static bool ContinueProgress(ulong current, ulong total)
        {
            return true;
        }

        private void LogCallbackException(string name, Exception exception)
        {
            Core.LogError(
                "Unhandled exception in InteractionHandler.{0}: {1}",
                name,
                exception
            );
        }
    }
}
