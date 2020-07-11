using System;
using Microsoft.EntityFrameworkCore;
using VrLifeServer.Database.DbModels;

namespace VrLifeServer.Database
{
    public class VrLifeDbContext : DbContext
    {
        private DatabaseConnectionStruct conn;
        private static Config _conf;

        public VrLifeDbContext()
        {
            if(_conf == null)
            {
                VrLifeServer.Init();
            }
            this.conn = _conf.Database;
        }

        public VrLifeDbContext(DatabaseConnectionStruct conn)
        {
            this.conn = conn;
        }

        public void SetConnectionStruct(DatabaseConnectionStruct conn)
        {
            this.conn = conn;
        }

        public static void SetConfig(Config conf)
        {
            _conf = conf;
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<AppData> AppData { get; set; }
    }
}
