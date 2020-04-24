using System;
using System.ComponentModel.DataAnnotations;

namespace VrLifeServer.Database.DbModels
{
    public class Account
    {
        [Key]
        public string Username { get; set; }
        public string Passphrase { get; set; }
    }
}
