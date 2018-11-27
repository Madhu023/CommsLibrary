using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace CommsLibrary
{
    class TCPClient<T> : ITCPClient<T>
        {
            Socket _handler;

            private T ConvertByteArrayToObject(ref byte[] bytes)
            {
                int size = Marshal.SizeOf(typeof(T));
                IntPtr ptr = Marshal.AllocHGlobal(size);

                Marshal.Copy(bytes, 0, ptr, size);

                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }

            private byte[] ConvertObjectToByteArray(ref T data)
            {
                try
                {
                    int size = Marshal.SizeOf(data);
                    byte[] bytes = new byte[size];

                    IntPtr ptr = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(data, ptr, true);
                    Marshal.Copy(ptr, bytes, 0, size);
                    Marshal.FreeHGlobal(ptr);
                    return bytes;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }

            public bool IsConnected()
            {
                return _handler.Connected;
            }

            public T Receive()
            {
                byte[] receivedBytes = new byte[Marshal.SizeOf(typeof(T))];

                if (IsConnected())
                {
                    _handler.Receive(receivedBytes);
                }

                return ConvertByteArrayToObject(ref receivedBytes);
            }

            public void Send(T data)
            {
                if (IsConnected())
                {
                    _handler.Send(ConvertObjectToByteArray(ref data), SocketFlags.None);
                }
            }

            public void Connect(string IPAddress, int port)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(System.Net.IPAddress.Parse(IPAddress), port);
                _handler.Connect(IPAddress, port);
            }

            public void Close()
            {
                _handler.Close();
            }

            public void Disconnect(bool reusePort)
            {
                _handler.Disconnect(reusePort);
            }

            public void BeginReceive()
            {
                StateObject state = new StateObject();
                state.handler = _handler;
                state.buffer = new byte[Marshal.SizeOf(typeof(T))];

                _handler.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
            }

            public void BeginSend(T data)
            {
                if (IsConnected())
                {
                    byte[] dataToSend = ConvertObjectToByteArray(ref data);
                    _handler.BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, new AsyncCallback(SendCallBack), _handler);
                }
            }

            TCPClient(SocketType socketType = SocketType.Stream)
            {
                _handler = new Socket(socketType, ProtocolType.Tcp);

            }

            private void SendCallBack(IAsyncResult ar)
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

            }

            private void ReceiveCallBack(IAsyncResult ar)
            {
                StateObject state = (StateObject)ar.AsyncState;
                int bytesReceived = state.handler.EndReceive(ar);

                var res = ConvertByteArrayToObject(ref state.buffer);
            }
        }
    }
}
