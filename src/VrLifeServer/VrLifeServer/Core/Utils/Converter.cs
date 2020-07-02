using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace VrLifeServer.Core.Utils
{
    public static class Converter
    {
        public static uint ToInt(this IPAddress addr)
        {
            byte[] bytes = addr.GetAddressBytes();

            // flip big-endian(network order) to little-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }
    }
}
