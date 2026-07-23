using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class WebsocketClient
    {
        private bool InvokeConnected(IntPtr context)
        {
            try
            {
                WebsocketConnectedDelegate? handler = this.connectedHandler;
                return null == handler || handler();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("connected", exception);
                return false;
            }
        }

        private void InvokeDisconnected(IntPtr context)
        {
            try
            {
                WebsocketDisconnectedDelegate? handler =
                    this.disconnectedHandler;
                if (null != handler)
                {
                    handler();
                }
            }
            catch (Exception exception)
            {
                this.LogCallbackException("disconnected", exception);
            }
        }

        private void InvokeError(string message, IntPtr context)
        {
            try
            {
                WebsocketErrorDelegate? handler = this.errorHandler;
                if (null != handler)
                {
                    handler(message);
                }
            }
            catch (Exception exception)
            {
                this.LogCallbackException("error", exception);
            }
        }

        private bool InvokeRead(IntPtr data, ulong length, IntPtr context)
        {
            try
            {
                WebsocketDataDelegate? handler = this.dataHandler;
                if (null == handler)
                {
                    return true;
                }

                byte[] managedData = new byte[checked((int)length)];
                if (0 != managedData.Length)
                {
                    Marshal.Copy(data, managedData, 0, managedData.Length);
                }

                return handler(managedData);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("read", exception);
                return false;
            }
        }

        private void LogCallbackException(string callback, Exception exception)
        {
            Core.LogError(
                "Unhandled exception in WebsocketClient {0} callback: {1}",
                callback,
                exception
            );
        }
    }
}
