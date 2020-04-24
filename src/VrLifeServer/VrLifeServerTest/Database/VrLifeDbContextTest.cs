using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using VrLifeServer.Database;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace VrLifeServerTest.Database
{
    public class VrLifeDbContextTest : IDisposable
    {

        public VrLifeDbContextTest()
        {
            VrLifeServer.VrLifeServer.Init();
        }

        [Fact]
        public void ConnectionTest()
        {
            using (var db = new VrLifeDbContext())
            {
                Assert.True(db.Database.CanConnect());
            }
        }

        [Fact]
        public void DatabaseCreatedTest()
        {
            using (var db = new VrLifeDbContext())
            {
                Assert.True((db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists());
            }
        }

        public void Dispose()
        {
            VrLifeServer.VrLifeServer.OnProcessExit(null, null);
        }
    }
}
