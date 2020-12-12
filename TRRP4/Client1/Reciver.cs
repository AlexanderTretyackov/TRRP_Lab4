using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Reciver
    {
        private readonly string ipAddress;
        private readonly int port;

        public Reciver(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            
        }

        public void BeginRecieve()
        {
            //создаем сокет для прослушивания запросов других клиентов
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Configs.LocalHost), Configs.ClientPort);

            // связываем сокет с локальной точкой, по которой будем приветствия от других клиентов
            listenSocket.Bind(ipPoint);

            // начинаем прослушивание приветствия других клиентов
            listenSocket.Listen(10);
            Console.WriteLine("Клиент ожидает подключения других клиентов...");
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
       
            try
            {
                var message = (Message)HelperClass.ByteArrayToObject(HelperClass.RecieveMessage(handler));
                //string query = site + info + $"&mode={mode}";
                
                //message = ReTry(query);
                Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                  $"{Thread.CurrentThread.Name} : " +
                                  $"The message is received");
                if(message.Command == Command.Greeting)
                    handler.Send(HelperClass.ObjectToByteArray(true));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString(new CultureInfo("ru-RU"))} " +
                                  $"{Thread.CurrentThread.Name} :" +
                                  ex.Message);
                //message = "Извените, но на данный момент невозможно получить ответ";
                //if (handler != null && handler.Connected)
                //    handler.Send(HelperClass.ObjectToByteArray(message));
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
