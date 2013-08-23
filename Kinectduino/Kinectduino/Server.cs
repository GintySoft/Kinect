using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.SPOT;

namespace GintySoft.Kinectduino
{
    public abstract class Server : IDisposable
    {
        public Socket Socket { get; set; }
        public virtual int Port
        {
            get
            {
                return -1;
            }
        }

        public Server()
        {  
            //Initialize Socket class
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Request and bind to an IP from DHCP server
            Socket.Bind(new IPEndPoint(IPAddress.Any, this.Port));
            //Debug print our IP address
            Debug.Print(Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);
            //Start listen for web requests
            Socket.Listen(10);
        }

        public void Start()
        {
            while (true)
            {
                using (Socket clientSocket = Socket.Accept())
                {
                    HandleRequest(clientSocket);
                }
            }
        }

        protected abstract void HandleRequest(Socket s);

         #region IDisposable Members
        ~Server()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (Socket != null)
                Socket.Close();
        }
        #endregion
    }
}
