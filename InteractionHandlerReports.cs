using System;

namespace BinaryNinja
{
    public abstract partial class InteractionHandler
    {
        private void InvokeShowPlainTextReport(
            IntPtr context,
            IntPtr view,
            string title,
            string contents
        )
        {
            BinaryView? managedView = null;
            try
            {
                managedView = NewView(view);
                this.ShowPlainTextReport(managedView, title, contents);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ShowPlainTextReport", exception);
            }
            finally
            {
                DisposeView(managedView);
            }
        }

        private void InvokeShowMarkdownReport(
            IntPtr context,
            IntPtr view,
            string title,
            string contents,
            string plainText
        )
        {
            BinaryView? managedView = null;
            try
            {
                managedView = NewView(view);
                this.ShowMarkdownReport(
                    managedView,
                    title,
                    contents,
                    plainText
                );
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ShowMarkdownReport", exception);
            }
            finally
            {
                DisposeView(managedView);
            }
        }

        private void InvokeShowHtmlReport(
            IntPtr context,
            IntPtr view,
            string title,
            string contents,
            string plainText
        )
        {
            BinaryView? managedView = null;
            try
            {
                managedView = NewView(view);
                this.ShowHTMLReport(managedView, title, contents, plainText);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ShowHtmlReport", exception);
            }
            finally
            {
                DisposeView(managedView);
            }
        }

        private void InvokeShowGraphReport(
            IntPtr context,
            IntPtr view,
            string title,
            IntPtr graph
        )
        {
            BinaryView? managedView = null;
            FlowGraph? managedGraph = null;
            try
            {
                managedView = NewView(view);
                managedGraph = FlowGraph.TakeHandle(
                    NativeMethods.BNNewFlowGraphReference(graph)
                );
                this.ShowGraphReport(managedView, title, managedGraph!);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ShowGraphReport", exception);
            }
            finally
            {
                if (null != managedGraph)
                {
                    managedGraph.Dispose();
                }

                DisposeView(managedView);
            }
        }

        private void InvokeShowReportCollection(
            IntPtr context,
            string title,
            IntPtr reports
        )
        {
            ReportCollection? managedReports = null;
            try
            {
                managedReports = ReportCollection.NewFromHandle(reports);
                this.ShowReportCollection(title, managedReports!);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("ShowReportCollection", exception);
            }
            finally
            {
                if (null != managedReports)
                {
                    managedReports.Dispose();
                }
            }
        }

        private static BinaryView? NewView(IntPtr view)
        {
            if (IntPtr.Zero == view)
            {
                return null;
            }

            return BinaryView.TakeHandle(NativeMethods.BNNewViewReference(view));
        }

        private static void DisposeView(BinaryView? view)
        {
            if (null != view)
            {
                view.Dispose();
            }
        }
    }
}
