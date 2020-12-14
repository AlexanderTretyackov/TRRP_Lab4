using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static Client.MainForm;

namespace Client
{
    public class Client
    {
        /// <summary>
        /// Уровень сложности задачи, решаемой клиентами
        /// </summary>
        const int levelTask = 10000;
        public static ConcurrentDictionary<string, IPEndPoint> otherClients =
            new ConcurrentDictionary<string, IPEndPoint>();
        /// <summary>
        /// Словарь клиентов, которые выполняют на данный момент задания с исходными данными MessageData
        /// </summary>
        public static ConcurrentDictionary<string, MessageData> clientsWorks =
            new ConcurrentDictionary<string, MessageData>();
        /// <summary>
        /// Словарь ip адресов клиентов, выполняющих работу и соответствующих им сокетов
        /// </summary>
        public static ConcurrentDictionary<string, Socket> clientsSockets =
            new ConcurrentDictionary<string, Socket>();
        /// <summary>
        /// Ответы на подзадачи
        /// </summary>
        public static ConcurrentBag<int> answersValues =
            new ConcurrentBag<int>();
        /// <summary>
        /// Количество частей на которые поделили работу
        /// </summary>
        int countPartsWork;
        /// <summary>
        /// Количество частей работы которые остались не отданными, будем решать сами
        /// </summary>
        int countPartsWorkForMe;
        public static bool loaded = false;
        static bool cancelled = false;
        public Client()
        {
            Task.Run(() => Greeting());
            Task.Run(() => Reconnect());
        }

        public void Cancel()
        {
            cancelled = true;
            foreach (var socket in clientsSockets)
                socket.Value.Close();
            //if (socket == null) return;
            //socket.Close();
            //socket = null;
        }

        public static int GetLocalNum()
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
        /// Получает ip адрес компьютера в локальной сети
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
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
        /// переподключаемся
        /// </summary>
        public void Reconnect()
        {
            while (true)
            {
                List<IPAddress> list = new List<IPAddress>();
                foreach (var client in otherClients.Keys)
                    list.Add(IPAddress.Parse(client));
                DoWorkLoads(list).Wait();
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Здороваемся со всеми клиентами в сети
        /// </summary>
        /// 
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
                //Console.WriteLine("testing : " + ip);

                bool result = SendHelloToClient(ip.ToString(), Configs.ClientPort);

                if (result)
                    otherClients.TryAdd(ip.ToString(),
                        new IPEndPoint(ip, Configs.ClientPort));
                else
                {
                    IPEndPoint iPEndPoint;
                    otherClients.TryRemove(ip.ToString(), out iPEndPoint);
                    //Console.WriteLine($"УДАЛЕН {ip}");
                }

            }
            catch
            {
                //Console.WriteLine("catch ex");
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

        public Message StartWorking()
        {
            Socket socket = null;
            string ipAddress = "10.147.20.151";
            int port = 8000;
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
                            Command = Command.Work,
                            Data = new MessageData
                            {
                                A = 123
                            }
                        }));

                    return (Message)HelperClass.ByteArrayToObject(HelperClass.RecieveMessage(socket));
                }
                else
                {
                    socket.Close();
                    return null;
                    //throw new ApplicationException("Failed to connect");
                }
            }
            catch
            {
                //Console.WriteLine($"stop working{socket.RemoteEndPoint}");
                socket.Close();
                return null;
            }
        }

        /// <summary>
        /// Отправляет приветствие другому клиенту
        /// </summary>
        /// <param name="ipAddress">ip адрес другого клиента</param>
        /// <param name="port">порт на котором работает другой клиент</param>
        /// <returns></returns>
        /// 
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
                    return false;
                    //throw new ApplicationException("Failed to connect");
                }
            }
            catch
            {
                socket.Close();
                return false;
            }
        }

        /// <summary>
        /// Распределяем работу между доступными клиентами
        /// </summary>
        public void DistributeWork()
        {
            //очищаем список сокетов
            clientsSockets.Clear();
            //вычисляем часть задачи, которая будет выдана для решения другим клиентам
            //при этом себя тоже учитываем как исполнителя
            var partOfWork = levelTask / (otherClients.Count + 1);
            countPartsWork = otherClients.Count + 1;
            int a = 0;
            foreach (var client in otherClients)
            {
                if (cancelled)
                    return;
                SendPartWorkToClient(client.Key, Configs.ClientPort,
                    new MessageData
                    {
                        A = a,
                        B = a + partOfWork,
                    });
                a += partOfWork;
            }
            //начинаем сами решать свою часть задачи
            RunPartOwnWork(new MessageData
            {
                A = a,
                B = levelTask,
            });

            Task.Run(() =>
            {
                //ждем пока не выполнятся все задачи
                while (countPartsWork != answersValues.Count) { }
                CurrentForm.output.BeginInvoke(
                    new InvokeDelegate(
                () =>
                {
                    var result = 0;
                    foreach (var answerValue in answersValues)
                        result += answerValue;
                    CurrentForm.output.Text += $"\nЗадача успешно решена, контрольная сумма{result}/{levelTask}";
                    CurrentForm.btnCancel.Enabled = false;
                    CurrentForm.btSend.Enabled = true;
                }));
            });
        }

        /// <summary>
        /// Запускаем решение части задачи у себя
        /// </summary>
        void RunPartOwnWork(MessageData messageData)
        {
            Task.Run(() =>
            {
                var answer = Worker.DoWork(messageData);
                answersValues.Add(answer.Value);
                CurrentForm.output.BeginInvoke(new InvokeDelegate(
                () =>
                {
                    CurrentForm.output.Text += $"\nМы сами решили подзадачу и получили ответ {answer.Value}";
                }));
                Console.WriteLine($"\nМы сами решили подзадачу и получили ответ {answer.Value}");
            });
        }

        /// <summary>
        /// Просим другого клиента выполнить часть работы
        /// </summary>
        /// <param name="ipAddress">ip адрес другого клиента</param>
        /// <param name="port">Порт другого клиента</param>
        /// <param name="messageData">Часть работы</param>
        void SendPartWorkToClient(string ipAddress, int port, MessageData messageData)
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
                    //отправляем клиенту данные для решения задачи
                    socket.Send(HelperClass.ObjectToByteArray(
                        new Message
                        {
                            Command = Command.Work,
                            Data = messageData,
                        }));
                    //добавляем в список сокетов клиентов новый
                    clientsSockets.TryAdd(ipAddress, socket);
                    clientsWorks.TryAdd(ipAddress, messageData);
                    //ждем ответ на подзадачу
                    var answer = (Answer)HelperClass.ByteArrayToObject(HelperClass.RecieveMessage(socket));
                    if (answer.DoneWork)
                        answersValues.Add(answer.Value);
                    CurrentForm.output.BeginInvoke(new InvokeDelegate(
                    () =>
                    {
                        CurrentForm.output.Text += $"\nКлиент {ipAddress} решил подзадачу и получил ответ {answer.Value}";
                    }));
                    Console.WriteLine($"\nКлиент {ipAddress} решил подзадачу и получил ответ {answer.Value}");
                }
                else
                {
                    socket.Close();
                    CurrentForm.output.BeginInvoke(new InvokeDelegate(
                    () =>
                    {
                        CurrentForm.output.Text += $"\nКлиент {ipAddress} не смог решить свою часть задачи";
                    }));
                    Console.WriteLine($"\nКлиент {ipAddress} не смог решить свою часть задачи");
                    //надо переназначить часть работы кому-то другому
                    MessageData messageData1 = null;
                    if (clientsWorks.TryGetValue(ipAddress, out messageData1))
                        RunPartOwnWork(messageData1);
                }
            }
            catch
            {
                socket.Close();

                CurrentForm.output.BeginInvoke(new InvokeDelegate(
                () =>
                {
                   CurrentForm.output.Text += $"\nКлиент {ipAddress} не смог решить свою часть задачи";
                }));
                
                Console.WriteLine($"\nКлиент {ipAddress} не смог решить свою часть задачи");
                //если другой клиент не смог взять задачу на себя
                MessageData messageData1 = null;
                if (clientsWorks.TryGetValue(ipAddress, out messageData1))
                    RunPartOwnWork(messageData1);
            }
        }
    }
}
