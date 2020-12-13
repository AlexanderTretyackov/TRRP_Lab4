using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [Serializable]
    public class Answer
    {
        /// <summary>
        /// Значение ответа на подзадачу
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Флаг смог ли клиент завершить решение подзадачи
        /// </summary>
        public bool DoneWork { get; set; }
    }
}
