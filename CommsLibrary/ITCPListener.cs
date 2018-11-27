namespace CommsLibrary
{
    public interface ITCPListener<T>
    {
        bool IsConnected();

        void Connect(string IPAddress, int port);

        void Send(T data);

        T Receive();

        void BeginReceive();

        void BeginSend(T data);

        void Disconnect(bool reusePort);

        void Close();
    }
}
