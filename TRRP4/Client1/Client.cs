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
        private static readonly IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("192.168.0.3"), 5555);

        public Result SendSocket(int[,] matrix)
        {      
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к диспетчеру
                socket.Connect(ipPoint);
                //отправляем диспетчеру исходные данные для алгоритма
                socket.Send(HelperClass.ObjectToByteArray(matrix));
                return (Result)HelperClass.ByteArrayToObject(HelperClass.RecieveMessage(socket));
            }
            catch
            {
                return new Result
                {
                    Success = false,
                    Message = "Не удалось получить ответ",
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
