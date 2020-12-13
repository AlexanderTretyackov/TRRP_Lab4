using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Worker
    {
        /// <summary>
        /// Занят ли работник выполнением задачи
        /// </summary>
        public static bool IsBusy = false;
        //Список исходных данных для подзадач, которые надо решить самому
        public static List<MessageData> messageDatas { get; set; } = new List<MessageData>();
        public static Answer DoWork(MessageData messageData)
        {
            IsBusy = true;
            //вычисляем размер пришедшей подзадачи
            var partWork = messageData.B - messageData.A;
            //имитируем решение подзадачи в зависимости от ее сложности по формуле 
            //(абстрастная сложность задачи * на размер подзадачи)
            //Thread.Sleep(Configs.levelWork * (partWork));
            Thread.Sleep(5000);
            IsBusy = false;
            return new Answer
            {
                DoneWork = true,
                Value = partWork,
            };         
        }
    }
}
