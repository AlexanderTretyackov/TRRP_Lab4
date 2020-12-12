using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Client
{
    public static class HelperClass
    {
        //public static int port = 8005;

        //public static string address = "192.168.0.3";

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
        public static byte[] RecieveMessage(Socket socket)
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
    }
}
