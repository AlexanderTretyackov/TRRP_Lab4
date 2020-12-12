using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [Serializable]
    public enum Command{
        Greeting,
        Work
    }
    [Serializable]
    class Message
    {
        public Command Command { get; set; }
        public MessageData Data { get; set; }
    }
}
