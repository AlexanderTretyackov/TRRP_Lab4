using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

namespace ServerSocket
{
    public static class HelperClass
    {
        public static int port = 8005;

        public static string address = "192.168.0.3";

        #region Сериализация
        public static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
        #endregion

        #region Сокеты
        public static byte[] RecieveMes(Socket socket)
        {
            // получаем сообщение
            List<byte> builder = new List<byte>();
            int bytes = 0; // количество полученных байтов
            byte[] data = new byte[256]; // буфер для получаемых данных
            do
            {
                bytes = socket.Receive(data);
                for (int i = 0; i < bytes; i++)
                    builder.Add(data[i]);
            } while (socket.Available > 0);

            return builder.ToArray();
        }
        #endregion

        #region HTTP
        public static T sendRequest<T>(string site,
            string contentType = null,
            bool authorization = false,
            string method = WebRequestMethods.Http.Get,
            byte[] body = null,
            WebHeaderCollection header = null)
        {
            return JsonConvert.DeserializeObject<T>(sendRequest(site, contentType, authorization, method, body, header));
        }

        public static string sendRequest(string site,
            string contentType = null,
            bool authorization = false,
            string method = WebRequestMethods.Http.Get,
            byte[] body = null,
            WebHeaderCollection header = null)
        {
            var request = HttpWebRequest.Create(site);

            if (header != null)
                request.Headers = header;

            request.Method = method;

            if (body != null)
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Flush();
                    stream.Close();
                }
            }

            if (authorization)
                request.Headers[HttpRequestHeader.Authorization] = "Bearer ";

            if (contentType != null)
                request.ContentType = contentType;
            var response = (HttpWebResponse)request.GetResponse();

            string answerJson;

            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                answerJson = reader.ReadToEnd();
            }

            return answerJson;
        }
        #endregion

    }
}
