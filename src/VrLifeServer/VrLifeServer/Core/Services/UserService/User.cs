using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VrLifeServer.Database;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    public class User
    {
        private Database.DbModels.User _dbUser;

        public ulong Id { get => _dbUser.UserId; }
        public string Username { get => _dbUser.Username; }

        public User(Database.DbModels.User dbUser)
        {
            if(dbUser == null)
            {
                throw new ArgumentNullException("Argumen 'dbUser' cannot be null.");
            }
            this._dbUser = dbUser;
        }

        public User(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException("Argument 'user' cannot be null.");
            }
            this._dbUser = user._dbUser;
        }

        public User(UserDetailMsg msg)
        {
            if(msg == null)
            {
                throw new ArgumentNullException("Argument 'user' cannot be null.");
            }
            _dbUser = new Database.DbModels.User();
            _dbUser.UserId = msg.UserId;
            _dbUser.Username = msg.Username;
            _dbUser.Passphrase = msg.Password;
        }

        public bool CheckPassword(string password)
        {
            return _dbUser.Passphrase == password;
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            if(!CheckPassword(oldPassword))
            {
                throw new UserException("Invalid old password.");
            }
            using(var db = new VrLifeDbContext())
            {
                db.Attach(this._dbUser);
                _dbUser.Passphrase = newPassword;
                db.SaveChanges();
            }
        }

        public static User Register(string username, string passphrase)
        {
            using (var db = new VrLifeDbContext())
            {
                Database.DbModels.User user = new Database.DbModels.User();
                user.Username = username;
                user.Passphrase = passphrase;
                db.Users.Add(user);
                db.SaveChanges();
                return new User(user);
            }
        }

        public void Delete()
        {
            using (var db = new VrLifeDbContext())
            {
                db.Attach(this._dbUser);
                db.Users.Remove(this._dbUser);
                db.SaveChanges();
            }
        }

        public static User Get(ulong userId)
        {
            using (var db = new VrLifeDbContext())
            {
                var dbUser = db.Users.SingleOrDefault(x => x.UserId == userId);
                return dbUser == null ? null : new User(dbUser);
            }
        }

        public static User Get(string username)
        {
            using (var db = new VrLifeDbContext())
            {
                var dbUser = db.Users.SingleOrDefault(x => x.Username == username);
                return dbUser == null ? null : new User(dbUser);
            }
        }

        public UserDetailMsg ToMessage()
        {
            UserDetailMsg userDetails = new UserDetailMsg();
            userDetails.UserId = Id;
            userDetails.Username = Username;

            return userDetails;
        }

        public static User[] List(UserDetailMsg query)
        {
            using (var db = new VrLifeDbContext())
            {
                var dbUsers = db.Users.Where(x => x.Username.Contains(query.Username));
                return dbUsers.Select(x => new User(x)).ToArray();
            }
        }
    }
}
