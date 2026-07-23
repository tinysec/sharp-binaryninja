namespace BinaryNinja
{
    public delegate bool WebsocketConnectedDelegate();

    public delegate void WebsocketDisconnectedDelegate();

    public delegate void WebsocketErrorDelegate(string message);

    public delegate bool WebsocketDataDelegate(byte[] data);
}
