using System;
namespace VrLifeServer.DbModels
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Passphrase { get; set; }
        public string OAuth { get; set; }
    }
}
