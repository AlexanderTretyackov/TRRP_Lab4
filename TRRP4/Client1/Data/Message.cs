﻿using System;

namespace Client
{
    [Serializable]
    public enum Command
    {
        Greeting,
        Work
    }
    [Serializable]
    public class Message
    {
        public Command Command { get; set; }
        public MessageData Data { get; set; }
    }
}
