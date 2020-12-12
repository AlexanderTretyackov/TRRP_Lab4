using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server;
using ServerSocket;
using System.Collections.Concurrent;

namespace Client
{
    public class Client
    {
        private static Socket socket = null;
        static ConcurrentBag<IPEndPoint> otherClients = new ConcurrentBag<IPEndPoint>();
        public Client()
        {
            Greeting();
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




        /// <summary>
        /// 
        /// </summary>
        public void Greeting()
        {
            int localNum = GetLocalNum();
            for (int i = 0; i < 256; i++)
            {
                if (i == localNum) continue;
                var address = $"{Configs.Mask}{i}";

                bool result = SendHelloToClient(address, Configs.ClientPort);

                if (result)
                    otherClients.Add(
                        new IPEndPoint(IPAddress.Parse(address), Configs.ClientPort));
            }
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


        public bool SendHelloToClient(string ipAddress, int port)
        {
            try
            {
                Socket socket = null;
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к другому клиенту
                socket.Connect(ipPoint);
                //отправляем клиенту приветствие
                socket.Send(HelperClass.ObjectToByteArray("hello"));
                //
                return (bool)HelperClass.ByteArrayToObject(HelperClass.RecieveMessage(socket));
            }
            catch
            {
                return false;
            }
        }
    }
}
