using Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerSocket
{
    class Program
    {
        static List<IPEndPoint> serverEndpoints = new List<IPEndPoint>() 
        { 
            new IPEndPoint(IPAddress.Parse("192.168.0.3"), 12345),
        };
        static int currentNumberServerEndpoint;
        static void Main(string[] args)
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("192.168.0.3"), 5555);
            currentNumberServerEndpoint = 0;

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
                currentNumberServerEndpoint = currentNumberServerEndpoint >= serverEndpoints.Count() ?
                    currentNumberServerEndpoint = 0:
                    currentNumberServerEndpoint++;
            }
        }
       
        private static void ReturnAnswer(object socket)
        {
            var clientSoketHandler = socket as Socket;

            try
            {
                Socket serverSocket = null;
                IPEndPoint ipPoint = serverEndpoints[currentNumberServerEndpoint];

                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к серверу
                serverSocket.Connect(ipPoint);
                //пересылаем данные клиента на сервер
                serverSocket.Send(HelperClass.RecieveMes(clientSoketHandler));
                var result = (Result)HelperClass.ByteArrayToObject(HelperClass.RecieveMes(serverSocket));
 
                //return new Result
                //{
                //    Success = false,
                //    Message = "Извините, но на данный момент невозможно получить ответ",
                //};
   
                Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                  $"{Thread.CurrentThread.Name} : " +
                                  $"The message is received");
                clientSoketHandler.Send(HelperClass.ObjectToByteArray(result));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                  $"{Thread.CurrentThread.Name} :" +
                                  ex.Message);
                //message = "Извените, но на данный момент невозможно получить ответ";
                //if (clientSoketHandler != null && clientSoketHandler.Connected)
                //    clientSoketHandler.Send(HelperClass.ObjectToByteArray(message));
            }

            finally
            {
                if (clientSoketHandler != null && clientSoketHandler.Connected)
                {
                    // закрываем сокет
                    clientSoketHandler.Shutdown(SocketShutdown.Both);
                    clientSoketHandler.Close();
                }
            }
        }

        //private static string ReTry(string query, int count = 5, int startinterval = 1000)
        //{
        //    string answer = "Error";
        //    var exeptions = new HashSet<Exception>();
        //    for (int attemptNum = 1; attemptNum <= count; attemptNum++)
        //    {
        //        try
        //        {
        //            Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
        //                              $"{Thread.CurrentThread.Name} : " +
        //                              $"Try to connect. Attempt №{attemptNum}");
        //            answer = HelperClass.sendRequest(query);
        //            return answer;
        //        }
        //        catch (Exception ex)
        //        {
        //            exeptions.Add(ex);
        //            Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " + 
        //                              $"{Thread.CurrentThread.Name} :" + 
        //                              ex.Message);
        //            Thread.Sleep(startinterval * attemptNum);
        //        }
        //    }

        //    throw new AggregateException(exeptions);
        //}
    }
}
