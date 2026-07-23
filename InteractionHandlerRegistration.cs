using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class InteractionHandler
    {
        private NativeDelegates.BNInteractionReport? plainReportCallback;
        private NativeDelegates.BNInteractionRichReport? markdownReportCallback;
        private NativeDelegates.BNInteractionRichReport? htmlReportCallback;
        private NativeDelegates.BNInteractionGraphReport? graphReportCallback;
        private NativeDelegates.BNInteractionReportCollection?
            reportCollectionCallback;
        private NativeDelegates.BNInteractionStringInput? textInputCallback;
        private NativeDelegates.BNInteractionIntegerInput? integerInputCallback;
        private NativeDelegates.BNInteractionAddressInput? addressInputCallback;
        private NativeDelegates.BNInteractionChoiceInput? choiceInputCallback;
        private NativeDelegates.BNInteractionChoiceInput? largeChoiceInputCallback;
        private NativeDelegates.BNInteractionStringInput? openFileInputCallback;
        private NativeDelegates.BNInteractionSaveFileInput? saveFileInputCallback;
        private NativeDelegates.BNInteractionStringInput? directoryInputCallback;
        private NativeDelegates.BNInteractionCheckboxInput? checkboxInputCallback;
        private NativeDelegates.BNInteractionFormInput? formInputCallback;
        private NativeDelegates.BNInteractionMessageBox? messageBoxCallback;
        private NativeDelegates.BNInteractionOpenUrl? openUrlCallback;
        private NativeDelegates.BNInteractionProgressDialog? progressDialogCallback;

        private void InitializeCallbacks()
        {
            this.plainReportCallback = new NativeDelegates.BNInteractionReport(
                this.InvokeShowPlainTextReport
            );
            this.markdownReportCallback =
                new NativeDelegates.BNInteractionRichReport(
                    this.InvokeShowMarkdownReport
                );
            this.htmlReportCallback =
                new NativeDelegates.BNInteractionRichReport(
                    this.InvokeShowHtmlReport
                );
            this.graphReportCallback =
                new NativeDelegates.BNInteractionGraphReport(
                    this.InvokeShowGraphReport
                );
            this.reportCollectionCallback =
                new NativeDelegates.BNInteractionReportCollection(
                    this.InvokeShowReportCollection
                );
            this.textInputCallback =
                new NativeDelegates.BNInteractionStringInput(
                    this.InvokeGetTextLineInput
                );
            this.integerInputCallback =
                new NativeDelegates.BNInteractionIntegerInput(
                    this.InvokeGetIntegerInput
                );
            this.addressInputCallback =
                new NativeDelegates.BNInteractionAddressInput(
                    this.InvokeGetAddressInput
                );
            this.choiceInputCallback =
                new NativeDelegates.BNInteractionChoiceInput(
                    this.InvokeGetChoiceInput
                );
            this.largeChoiceInputCallback =
                new NativeDelegates.BNInteractionChoiceInput(
                    this.InvokeGetLargeChoiceInput
                );
            this.openFileInputCallback =
                new NativeDelegates.BNInteractionStringInput(
                    this.InvokeGetOpenFileNameInput
                );
            this.saveFileInputCallback =
                new NativeDelegates.BNInteractionSaveFileInput(
                    this.InvokeGetSaveFileNameInput
                );
            this.directoryInputCallback =
                new NativeDelegates.BNInteractionStringInput(
                    this.InvokeGetDirectoryNameInput
                );
            this.checkboxInputCallback =
                new NativeDelegates.BNInteractionCheckboxInput(
                    this.InvokeGetCheckboxInput
                );
            this.formInputCallback = new NativeDelegates.BNInteractionFormInput(
                this.InvokeGetFormInput
            );
            this.messageBoxCallback =
                new NativeDelegates.BNInteractionMessageBox(
                    this.InvokeShowMessageBox
                );
            this.openUrlCallback = new NativeDelegates.BNInteractionOpenUrl(
                this.InvokeOpenUrl
            );
            this.progressDialogCallback =
                new NativeDelegates.BNInteractionProgressDialog(
                    this.InvokeRunProgressDialog
                );
        }

        private BNInteractionHandlerCallbacks CreateCallbacks()
        {
            BNInteractionHandlerCallbacks callbacks =
                new BNInteractionHandlerCallbacks();
            callbacks.context = IntPtr.Zero;
            callbacks.showPlainTextReport = Pointer(this.plainReportCallback!);
            callbacks.showMarkdownReport = Pointer(this.markdownReportCallback!);
            callbacks.showHTMLReport = Pointer(this.htmlReportCallback!);
            callbacks.showGraphReport = Pointer(this.graphReportCallback!);
            callbacks.showReportCollection = Pointer(
                this.reportCollectionCallback!
            );
            callbacks.getTextLineInput = Pointer(this.textInputCallback!);
            callbacks.getIntegerInput = Pointer(this.integerInputCallback!);
            callbacks.getAddressInput = Pointer(this.addressInputCallback!);
            callbacks.getChoiceInput = Pointer(this.choiceInputCallback!);
            callbacks.getLargeChoiceInput = Pointer(
                this.largeChoiceInputCallback!
            );
            callbacks.getOpenFileNameInput = Pointer(
                this.openFileInputCallback!
            );
            callbacks.getSaveFileNameInput = Pointer(
                this.saveFileInputCallback!
            );
            callbacks.getDirectoryNameInput = Pointer(
                this.directoryInputCallback!
            );
            callbacks.getCheckboxInput = Pointer(this.checkboxInputCallback!);
            callbacks.getFormInput = Pointer(this.formInputCallback!);
            callbacks.showMessageBox = Pointer(this.messageBoxCallback!);
            callbacks.openUrl = Pointer(this.openUrlCallback!);
            callbacks.runProgressDialog = Pointer(
                this.progressDialogCallback!
            );

            return callbacks;
        }

        private static IntPtr Pointer<TDelegate>(TDelegate callback)
            where TDelegate : Delegate
        {
            return Marshal.GetFunctionPointerForDelegate<TDelegate>(callback);
        }
    }
}
