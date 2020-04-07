using System;
namespace VrLifeServer.Database
{
    public struct DatabaseConnectionStruct
    {
        public string Type;
        public string Host;
        public int Port;
        public string Username;
        public string Password;
        public string Database;
    }
}
