using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
    /// <summary>Represents a WebSocket client created by a provider.</summary>
    public abstract partial class WebsocketClient :
        AbstractSafeHandle<WebsocketClient>
    {
        private WebsocketConnectedDelegate? connectedHandler;
        private WebsocketDisconnectedDelegate? disconnectedHandler;
        private WebsocketErrorDelegate? errorHandler;
        private WebsocketDataDelegate? dataHandler;
        private NativeDelegates.BNWebsocketConnected? connectedCallback;
        private NativeDelegates.BNWebsocketDisconnected? disconnectedCallback;
        private NativeDelegates.BNWebsocketError? errorCallback;
        private NativeDelegates.BNWebsocketRead? readCallback;
        private bool connectionStarted;

        private protected WebsocketClient(IntPtr handle, bool owner)
            : base(handle, owner)
        {
        }

        internal static WebsocketClient? TakeHandle(IntPtr handle)
        {
            return IntPtr.Zero == handle
                ? null
                : new CoreWebsocketClient(handle);
        }

        /// <summary>Starts an asynchronous connection.</summary>
        public bool Connect(
            string url,
            IReadOnlyDictionary<string, string>? headers = null,
            WebsocketConnectedDelegate? onConnected = null,
            WebsocketDisconnectedDelegate? onDisconnected = null,
            WebsocketErrorDelegate? onError = null,
            WebsocketDataDelegate? onData = null
        )
        {
            if (this.connectionStarted)
            {
                throw new InvalidOperationException(
                    "A WebSocket client cannot connect more than once."
                );
            }

            this.connectionStarted = true;
            this.connectedHandler = onConnected;
            this.disconnectedHandler = onDisconnected;
            this.errorHandler = onError;
            this.dataHandler = onData;
            this.connectedCallback = new NativeDelegates.BNWebsocketConnected(
                this.InvokeConnected
            );
            this.disconnectedCallback =
                new NativeDelegates.BNWebsocketDisconnected(
                    this.InvokeDisconnected
                );
            this.errorCallback = new NativeDelegates.BNWebsocketError(
                this.InvokeError
            );
            this.readCallback = new NativeDelegates.BNWebsocketRead(
                this.InvokeRead
            );

            string[] keys;
            string[] values;
            CopyHeaders(headers, out keys, out values);
            BNWebsocketClientOutputCallbacks callbacks =
                new BNWebsocketClientOutputCallbacks();
            callbacks.context = IntPtr.Zero;
            callbacks.connectedCallback = Pointer(this.connectedCallback);
            callbacks.disconnectedCallback = Pointer(
                this.disconnectedCallback
            );
            callbacks.errorCallback = Pointer(this.errorCallback);
            callbacks.readCallback = Pointer(this.readCallback);

            using ScopedAllocator allocator = new ScopedAllocator();
            return NativeMethods.BNConnectWebsocketClient(
                this.handle,
                url,
                (ulong)keys.Length,
                allocator.AllocUtf8StringArray(keys),
                allocator.AllocUtf8StringArray(values),
                in callbacks
            );
        }

        /// <summary>Writes data to this client.</summary>
        public bool Write(byte[] data)
        {
            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            GCHandle pin = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return 0 != NativeMethods.BNWriteWebsocketClientData(
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

        /// <summary>Writes a raw buffer and returns the core result.</summary>
        public ulong WriteData(IntPtr data, ulong length)
        {
            return NativeMethods.BNWriteWebsocketClientData(
                this.handle,
                data,
                length
            );
        }

        /// <summary>Disconnects this client.</summary>
        public bool Disconnect()
        {
            return NativeMethods.BNDisconnectWebsocketClient(this.handle);
        }

        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                NativeMethods.BNFreeWebsocketClient(this.handle);
                this.SetHandleAsInvalid();
            }

            return true;
        }

        private static void CopyHeaders(
            IReadOnlyDictionary<string, string>? headers,
            out string[] keys,
            out string[] values
        )
        {
            if (null == headers || 0 == headers.Count)
            {
                keys = Array.Empty<string>();
                values = Array.Empty<string>();
                return;
            }

            keys = new string[headers.Count];
            values = new string[headers.Count];
            int index = 0;
            foreach (KeyValuePair<string, string> header in headers)
            {
                keys[index] = header.Key;
                values[index] = header.Value;
                index++;
            }
        }

        private static IntPtr Pointer<TDelegate>(TDelegate callback)
            where TDelegate : Delegate
        {
            return Marshal.GetFunctionPointerForDelegate(callback);
        }

        private sealed class CoreWebsocketClient : WebsocketClient
        {
            internal CoreWebsocketClient(IntPtr handle)
                : base(handle, true)
            {
            }

            protected override bool PerformConnect(
                string host,
                IReadOnlyDictionary<string, string> headers
            )
            {
                return false;
            }

            protected override bool PerformWrite(byte[] data)
            {
                return false;
            }

            protected override bool PerformDisconnect()
            {
                return false;
            }
        }
    }
}
