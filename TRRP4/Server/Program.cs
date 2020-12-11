using Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using VertexCoverProblem;

namespace ServerSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("192.168.0.3"), 12345);

            // связываем сокет с локальной точкой, по которой будем принимать данные
            listenSocket.Bind(ipPoint);

            // начинаем пsрослушивание
            listenSocket.Listen(10);
            Console.WriteLine("Сервер запущен. Ожидание подключений...");
            int cnt = 0;
            while (true)
            {
                var handler = listenSocket.Accept();
                // создаем новый поток
                var myThread = new Thread(ReturnAnswer)
                {
                    Name = cnt++.ToString()
                };
                myThread.Start(handler);
            }
        }

        private static void ReturnAnswer(object socket)
        {
            var handler = socket as Socket;
            string message = "Error";
            try
            {
                var graph = (int [,])HelperClass.ByteArrayToObject(HelperClass.RecieveMessage(handler));
                //string query = site + info + $"&mode={mode}";
                var explicitSolver = new ExplicitAlgorytm(graph);
                var vertexCover = explicitSolver.Solve();
                var answer = string.Join(",", vertexCover);
                //message = ReTry(query);
                Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                  $"{Thread.CurrentThread.Name} : " +
                                  $"The message is received");
                handler.Send(HelperClass.ObjectToByteArray(
                    new Result {
                        Success = true,
                        Message = answer,
                    }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                  $"{Thread.CurrentThread.Name} :" +
                                  ex.Message);
                message = "Извените, но на данный момент невозможно получить ответ";
                if (handler != null && handler.Connected)
                    handler.Send(HelperClass.ObjectToByteArray(message));
            }

            finally
            {
                if (handler != null && handler.Connected)
                {
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
        }     
    }
}
