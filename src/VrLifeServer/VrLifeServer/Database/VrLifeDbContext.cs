using System;
using Microsoft.EntityFrameworkCore;
using VrLifeServer.Database.DbModels;

namespace VrLifeServer.Database
{
    public class VrLifeDbContext : DbContext
    {
        private DatabaseConnectionStruct conn;

        public VrLifeDbContext()
        {
            this.conn = VrLifeServer.Conf.Database;
        }

        public VrLifeDbContext(DatabaseConnectionStruct conn)
        {
            this.conn = conn;
        }

        public void SetConnectionStruct(DatabaseConnectionStruct conn)
        {
            this.conn = conn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (conn.Type.ToLower())
            {
                case "mysql":
                    optionsBuilder.UseMySql(
                        $"server={conn.Host};port={conn.Port};database={conn.Database}" +
                        $";user={conn.Username};password={conn.Password}");
                    break;
                default:
                    throw new ArgumentException("Unknown database type.");
            }
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
