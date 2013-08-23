using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.SPOT;

namespace GintySoft.Kinectduino
{
    public class PachubeSocket
    {
        public PachubeSocket()

        {
        }

        public bool put(string value)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry("");
                IPAddress hostAddress = hostEntry.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(hostAddress, 80);
                using (var connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    connection.Connect(remoteEndPoint);
                    connection.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                    connection.SendTimeout = 5000;
                    const string CRLF = "\r\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(value);
                    var requestLine = "PUT /v2/feeds/" + "" + ".csv HTTP/1.1" + CRLF;
                    byte[] requestLineBuffer = Encoding.UTF8.GetBytes(requestLine);
                    var headers = "Host: " + "" + CRLF + "X-PachubeAPIKey: " + "" + CRLF + "Content-Type: text/csv" + CRLF + "Content-Length: " + buffer.Length + CRLF + CRLF;
                    byte[] headerBuffer = Encoding.UTF8.GetBytes(headers);
                    connection.Send(requestLineBuffer);
                    connection.Send(headerBuffer);
                    connection.Send(buffer);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
