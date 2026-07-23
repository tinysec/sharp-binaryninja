using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    public abstract partial class WebsocketClient
    {
        private static readonly object registrationLock = new object();

        private static readonly List<WebsocketClient> registeredClients =
            new List<WebsocketClient>();

        private NativeDelegates.BNWebsocketDestroyClient? destroyClientCallback;
        private NativeDelegates.BNWebsocketConnect? connectCallback;
        private NativeDelegates.BNWebsocketWrite? writeCallback;
        private NativeDelegates.BNWebsocketDisconnect? disconnectCallback;
        private bool factoryReferenceReleased;

        protected WebsocketClient(WebsocketProvider provider)
            : base(IntPtr.Zero, false)
        {
            if (null == provider)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.destroyClientCallback =
                new NativeDelegates.BNWebsocketDestroyClient(
                    this.InvokeDestroyClient
                );
            this.connectCallback = new NativeDelegates.BNWebsocketConnect(
                this.InvokeConnect
            );
            this.writeCallback = new NativeDelegates.BNWebsocketWrite(
                this.InvokeWrite
            );
            this.disconnectCallback = new NativeDelegates.BNWebsocketDisconnect(
                this.InvokeDisconnect
            );
            BNWebsocketClientCallbacks callbacks =
                new BNWebsocketClientCallbacks();
            callbacks.context = IntPtr.Zero;
            callbacks.destroyClient = Pointer(this.destroyClientCallback);
            callbacks.connect = Pointer(this.connectCallback);
            callbacks.write = Pointer(this.writeCallback);
            callbacks.disconnect = Pointer(this.disconnectCallback);

            IntPtr client = NativeMethods.BNInitWebsocketClient(
                provider.DangerousGetHandle(),
                in callbacks
            );
            if (IntPtr.Zero == client)
            {
                throw new InvalidOperationException(
                    "The WebSocket client could not be initialized."
                );
            }

            this.SetHandle(client);
            lock (WebsocketClient.registrationLock)
            {
                WebsocketClient.registeredClients.Add(this);
            }
        }

        internal void ReleaseFactoryReference()
        {
            lock (WebsocketClient.registrationLock)
            {
                if (this.factoryReferenceReleased)
                {
                    return;
                }

                this.factoryReferenceReleased = true;
                NativeMethods.BNFreeWebsocketClient(this.handle);
            }
        }

        protected abstract bool PerformConnect(
            string host,
            IReadOnlyDictionary<string, string> headers
        );

        protected abstract bool PerformWrite(byte[] data);

        protected abstract bool PerformDisconnect();

        protected virtual void PerformDestroy()
        {
        }

        protected bool NotifyConnected()
        {
            return NativeMethods.BNNotifyWebsocketClientConnect(this.handle);
        }

        protected void NotifyDisconnected()
        {
            NativeMethods.BNNotifyWebsocketClientDisconnect(this.handle);
        }

        protected void NotifyError(string message)
        {
            NativeMethods.BNNotifyWebsocketClientError(this.handle, message);
        }

        protected bool NotifyData(byte[] data)
        {
            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            GCHandle pin = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return NativeMethods.BNNotifyWebsocketClientReadData(
                    this.handle,
                    0 == data.Length ? IntPtr.Zero : pin.AddrOfPinnedObject(),
                    (ulong)data.Length
                );
            }
            finally
            {
                pin.Free();
            }
        }

        private void InvokeDestroyClient(IntPtr context)
        {
            try
            {
                this.PerformDestroy();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("destroy", exception);
            }
            finally
            {
                this.SetHandleAsInvalid();
                lock (WebsocketClient.registrationLock)
                {
                    WebsocketClient.registeredClients.Remove(this);
                }
            }
        }

        private bool InvokeConnect(
            IntPtr context,
            string host,
            ulong headerCount,
            IntPtr headerKeys,
            IntPtr headerValues
        )
        {
            try
            {
                string[] keys = UnsafeUtils.ReadUtf8StringArray(
                    headerKeys,
                    headerCount
                );
                string[] values = UnsafeUtils.ReadUtf8StringArray(
                    headerValues,
                    headerCount
                );
                Dictionary<string, string> headers =
                    new Dictionary<string, string>();
                for (int i = 0; i < keys.Length; i++)
                {
                    headers[keys[i]] = values[i];
                }

                return this.PerformConnect(host, headers);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("connect", exception);
                return false;
            }
        }

        private bool InvokeWrite(IntPtr data, ulong length, IntPtr context)
        {
            try
            {
                byte[] managedData = new byte[checked((int)length)];
                if (0 != managedData.Length)
                {
                    Marshal.Copy(data, managedData, 0, managedData.Length);
                }

                return this.PerformWrite(managedData);
            }
            catch (Exception exception)
            {
                this.LogCallbackException("write", exception);
                return false;
            }
        }

        private bool InvokeDisconnect(IntPtr context)
        {
            try
            {
                return this.PerformDisconnect();
            }
            catch (Exception exception)
            {
                this.LogCallbackException("disconnect", exception);
                return false;
            }
        }
    }
}
