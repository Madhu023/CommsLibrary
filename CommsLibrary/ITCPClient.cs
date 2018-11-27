using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommsLibrary
{
    public interface ITCPClient<T>
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
