using System.Net.Sockets;

namespace CommsLibrary
{
    public class StateObject
    {
        // Client  socket.  
        public Socket handler = null;

        // Receive buffer.  
        public byte[] buffer;
    }
}
