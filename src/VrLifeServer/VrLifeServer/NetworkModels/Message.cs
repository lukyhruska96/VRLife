using System;
using System.Net;

namespace VrLifeServer.NetworkModels
{
    public class Message
    {
        public IPEndPoint Target { get; set; }

        public static Message Encode(byte[] data)
        {
            return new Message();
        }

        public ReadOnlySpan<byte> Decode()
        {
            return new byte[0];
        }
    }
}
