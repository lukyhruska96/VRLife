using System;
namespace VrLifeAPI.Provider.Database
{

    /// <summary>
    /// Informace nutné k připojení se do DB
    /// </summary>
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
