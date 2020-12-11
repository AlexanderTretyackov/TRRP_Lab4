using System;
using System.Net;
using System.Net.Sockets;
using Server;
using ServerSocket;

namespace Client
{
    public class Client
    {
        private static Socket socket = null;
        private static readonly IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(HelperClass.address), HelperClass.port);

        public Result SendSocket(object input)
        {
            string number = input.ToString();
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                //Данные
                var matrix = new bool[3,3];
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        matrix[i,j] = i > 0;
                socket.Send(HelperClass.ObjectToByteArray(matrix));
                return (Result)HelperClass.ByteArrayToObject(HelperClass.RecieveMes(socket));
            }
            catch
            {
                return new Result
                {
                    Success = false,
                    Message = "Извените, но на данный момент невозможно получить ответ",
                };
            }
        }

        public void Cancel()
        {
            if (socket == null) return;
            socket.Close();
            socket = null;
        }
    }
}
