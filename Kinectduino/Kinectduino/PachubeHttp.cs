using System;
using System.Net;
using System.IO;
using Microsoft.SPOT;

namespace GintySoft.Kinectduino
{
    public class PachubeHttp
    {
        public PachubeHttp()
        {
        }

        public bool Post(string value)
        {
            string message = @"{""value"":""" + value + @"""}";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            var requestURI = @"http://kinectduinotest.cloudapp.net/api/temperature";
            using (var request = (HttpWebRequest)WebRequest.Create(requestURI))
            {
                request.Method = "POST";
                request.UserAgent = "Netduino";
                request.ContentType = "application/json; charset=utf-8"; 
                request.ContentLength = buffer.Length;
                request.Accept = "application/json"; 
                Stream s = request.GetRequestStream();
                s.Write(buffer, 0, buffer.Length);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }       
            }
        }
    }
}
