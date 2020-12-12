using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server;
using ServerSocket;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        static ConcurrentBag<IPEndPoint> otherClients = new ConcurrentBag<IPEndPoint>();
        public Client()
        {
            Greeting();

            var reciever = new Reciver(GetLocalIP(), Configs.ClientPort);
            Task.Run(() => reciever.BeginRecieve());
        }

        public void Cancel()
        {
            //if (socket == null) return;
            //socket.Close();
            //socket = null;
        }

        public int GetLocalNum()
        {
            string Host = Dns.GetHostName();
            int localNum = -1;
            var adressList = Dns.GetHostByName(Host).AddressList;

            try
            {
                foreach (var adress in adressList)
                {
                    if (adress.ToString().Contains(Configs.Mask))
                    {
                        localNum = int.Parse(adress.ToString().Replace(Configs.Mask, ""));
                        break;
                    }
                }
            }
            catch (Exception)
            {
                localNum = -1;
            }

            return localNum;
        }

        public string GetLocalIP()
        {
            string Host = Dns.GetHostName();
            var adressList = Dns.GetHostByName(Host).AddressList;

            try
            {
                foreach (var adress in adressList)
                {
                    if (adress.ToString().Contains(Configs.Mask))
                        return adress.ToString();
                }
            }
            catch (Exception)
            {
                return $"{Configs.Mask}{GetLocalNum()}";
            }

            return Configs.LocalHost;
        }




        /// <summary>
        /// Здороваемся со всеми клиентами в сети
        /// </summary>
        public void Greeting()
        {
            int localNum = GetLocalNum();
            bool result = SendHelloToClient("10.147.20.151", Configs.ClientPort);
            //for (int i = 0; i < 256; i++)
            //{
            //    if (i == localNum) continue;
            //    var address = $"{Configs.Mask}{i}";

            //    bool result = SendHelloToClient(address, Configs.ClientPort);

            //    if (result)
            //        otherClients.Add(
            //            new IPEndPoint(IPAddress.Parse(address), Configs.ClientPort));
            //}
        }

        public int BruteForce(int start, int end)
        {
            for (int i = start; i <= end; i++)
                if (i == Configs.pass)
                {
                    // send info
                    break;
                }
            return 0;
        }

        /// <summary>
        /// Отправляет приветствие другому клиенту
        /// </summary>
        /// <param name="ipAddress">ip адрес другого клиента</param>
        /// <param name="port">порт на котором работает другой клиент</param>
        /// <returns></returns>
        public bool SendHelloToClient(string ipAddress, int port)
        {
            Socket socket = null;

            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к другому клиенту

                IAsyncResult result = socket.BeginConnect(ipAddress, port, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                if (socket.Connected)
                {
                    socket.EndConnect(result);
                    //отправляем клиенту приветствие
                    Console.WriteLine("sas");
                    socket.Send(HelperClass.ObjectToByteArray(
                        new Message
                        {
                            Command = Command.Greeting,
                            Data = null
                        }));

                    return (bool)HelperClass.ByteArrayToObject(HelperClass.RecieveMessage(socket));
                }
                else
                {
                    socket.Close();
                    throw new ApplicationException("Failed to connect");
                }
            }
            catch
            {
                socket.Close();
                return false;
            }
        }
    }
}
