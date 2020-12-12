using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Client
{
    public class Client
    {
        public static ConcurrentDictionary<string, IPEndPoint> otherClients = 
            new ConcurrentDictionary<string, IPEndPoint>();
        public static bool loaded = false;
        public Client()
        {
            Task.Run(() => Greeting());
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
            var list = GenerateIPAddressList();

            // start the work load
            DoWorkLoads(list).Wait();
        }

        public List<IPAddress> GenerateIPAddressList()
        {
            int localNum = GetLocalNum();
            var addresses = new List<IPAddress>();
            for (int i = 0; i < 256; i++)
            {
                if (i == localNum) continue;
                var address = $"{Configs.Mask}{i}";
                addresses.Add(IPAddress.Parse(address));
            }
            return addresses;
        }

        public async Task TestSocket(IPAddress ip)
        {
            try
            {
                Console.WriteLine("testing : " + ip);

                bool result = SendHelloToClient(ip.ToString(), Configs.ClientPort);

                if (result)
                    otherClients.TryAdd(ip.ToString(),
                        new IPEndPoint(ip, Configs.ClientPort));
            }
            catch
            {
                Console.WriteLine("catch ex");
            }
            finally
            {
                if (ip.ToString() == $"{Configs.Mask}255")
                    loaded = true;
            }
        }

        public async Task DoWorkLoads(List<IPAddress> addresses)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 255
            };

            var block = new ActionBlock<IPAddress>(TestSocket, options);

            foreach (var ip in addresses)
                block.Post(ip);

            block.Complete();
            await block.Completion;

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

                bool success = result.AsyncWaitHandle.WaitOne(1000, true);

                if (socket.Connected)
                {
                    socket.EndConnect(result);
                    //отправляем клиенту приветствие
                    socket.Send(HelperClass.ObjectToByteArray(
                        new Message
                        {
                            Command = Command.Greeting,
                            Data = new MessageData
                            {
                                A = 123
                            }
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
