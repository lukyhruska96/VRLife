using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using VrLifeServer.Core.Services.UserService;
using VrLifeServer.Database;
using Xunit.Extensions.Ordering;
using System.Linq;

namespace VrLifeServerTest.Core
{
    [Order(2)]
    public class UserTest : IDisposable
    {
        public UserTest()
        {
            VrLifeServer.VrLifeServer.Init();
        }

        public void Dispose()
        {
            VrLifeServer.VrLifeServer.OnProcessExit(null, null);
        }

        [Fact]
        [Order(1)]
        public void DeleteTest()
        {
            var user = new VrLifeServer.Database.DbModels.User();
            using (var db = new VrLifeDbContext())
            {
                user.Username = "username";
                user.Passphrase = "password";
                db.Users.Add(user);
                db.SaveChanges();
            }
            ulong userId = user.UserId;
            User obj = new User(user);
            obj.Delete();
            using (var db = new VrLifeDbContext())
            {
                Assert.Null(db.Users.SingleOrDefault(x => x.UserId == userId));
            }
                
        }

        [Fact]
        [Order(2)]
        public void GetTest()
        {
            var user = new VrLifeServer.Database.DbModels.User();
            using (var db = new VrLifeDbContext())
            {
                user.Username = "username";
                user.Passphrase = "password";
                db.Users.Add(user);
                db.SaveChanges();
            }
            User obj = User.Get(user.UserId);
            Assert.NotNull(obj);
            obj.Delete();
        }

        [Fact]
        [Order(3)]
        public void RegisterTest()
        {
            User user = User.Register("username", "password");
            ulong userId = user.Id;
            Assert.NotNull(User.Get(userId));
            user.Delete();
            Assert.Null(User.Get(userId));
        }

        [Fact]
        [Order(4)]
        public void ChangePassword()
        {
            string username = "username";
            string password = "password";
            string newPassword = "newPassword";
            User user = User.Register(username, password);
            Assert.True(user.CheckPassword(password));
            user.ChangePassword(password, newPassword);
            Assert.True(user.CheckPassword(newPassword));
            user.Delete();
        }
    }
}
