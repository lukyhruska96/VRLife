using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VrLifeServer.Database.DbModels
{
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Passphrase { get; set; }
    }
}
