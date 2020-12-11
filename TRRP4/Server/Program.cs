using Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(HelperClass.address), HelperClass.port);

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
                var matrix = (int [,])HelperClass.ByteArrayToObject(HelperClass.RecieveMes(handler));
                //string query = site + info + $"&mode={mode}";

                //message = ReTry(query);
                Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                  $"{Thread.CurrentThread.Name} : " +
                                  $"The message is received");
                handler.Send(HelperClass.ObjectToByteArray(
                    new Result {
                        Success = true,
                        Message = "1,2,3",
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

        private static string ReTry(string query, int count = 5, int startinterval = 1000)
        {
            string answer = "Error";
            var exeptions = new HashSet<Exception>();
            for (int attemptNum = 1; attemptNum <= count; attemptNum++)
            {
                try
                {
                    Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                      $"{Thread.CurrentThread.Name} : " +
                                      $"Try to connect. Attempt №{attemptNum}");
                    answer = HelperClass.sendRequest(query);
                    return answer;
                }
                catch (Exception ex)
                {
                    exeptions.Add(ex);
                    Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                      $"{Thread.CurrentThread.Name} :" +
                                      ex.Message);
                    Thread.Sleep(startinterval * attemptNum);
                }
            }

            throw new AggregateException(exeptions);
        }
    }
}
