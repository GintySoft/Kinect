using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;

namespace GintySoft.Kinectduino
{
    public class HttpWebServer : Server
    {
        public override int Port
        {
            get
            {
                return 80;
            }
        }
        public HttpWebServer() : base()
        {
          
        }

        protected override void HandleRequest(Socket s)
        {
            //Get clients IP
            IPEndPoint clientIP = s.RemoteEndPoint as IPEndPoint;
            EndPoint clientEndPoint = s.RemoteEndPoint;
            //int byteCount = cSocket.Available;
            int bytesReceived = s.Available;
            if (bytesReceived > 0)
            {
                //Get request
                byte[] buffer = new byte[bytesReceived];
                int byteCount = s.Receive(buffer, bytesReceived, SocketFlags.None);
                string request = new string(Encoding.UTF8.GetChars(buffer));
                Debug.Print(request);
                //Compose a response
                string response = "F";
                string header = "HTTP/1.0 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: " + response.Length.ToString() + "\r\nConnection: close\r\n\r\n";
                s.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
                s.Send(Encoding.UTF8.GetBytes(response), response.Length, SocketFlags.None);
                Thread.Sleep(150);
            }
        }
    }
}
