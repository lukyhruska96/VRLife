using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using VrLifeServer.Database;
using VrLifeServer.Database.DbModels;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    struct Token : IEquatable<Token>
    {

        public string TokenString;
        public DateTime ExpirationTime;

        public Token(double hours)
        {
            this.TokenString = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            this.ExpirationTime = DateTime.Now.AddHours(hours);
        }

        public override bool Equals(Object obj)
        {
            return obj is Token && Equals((Token)obj);
        }

        public bool Equals([AllowNull] Token other)
        {
            return other.TokenString == this.TokenString && other.ExpirationTime == this.ExpirationTime;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Token a, Token b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Token a, Token b) 
        {
            return !a.Equals(b);
        }
    }

    struct Credentials
    {
        public string username;
        public string passphrase;
    }
    class User
    {

        public string Username { get => username; }
        private string username;

        public Token token;

        private static ConcurrentDictionary<string, Token> authTokens = new ConcurrentDictionary<string, Token>();

        private static Token? Auth(Credentials credentials)
        {
            using (var db = new VrLifeDbContext())
            {
                var result = db.Accounts
                    .Where(x => x.Username == credentials.username 
                        && x.Passphrase == credentials.passphrase)
                    .ToList();
                if(result.Count == 0)
                {
                    return null;
                }
                return new Token(2.0);
            }
        }

        private static bool IsAuthorized(string username, Token token)
        {
            return authTokens.ContainsKey(username) && authTokens[username] == token && authTokens[username].ExpirationTime > DateTime.Now;
        }

        private static Token? RefreshToken(string username, Token token)
        {
            if(!IsAuthorized(username, token))
            {
                return null;
            }
            Token newToken = new Token(2.0);
            authTokens[username] = newToken;
            return newToken;
        }

        private static void Register(string username, string passphrase)
        {
            using (var db = new VrLifeDbContext())
            {
                Account account = new Account();
                account.Username = username;
                account.Passphrase = passphrase;
                db.Accounts.Add(account);
                db.SaveChanges();
            }
        }

        public MainMessage HandleMsg(UserMsg msg)
        {
            return null;
        }

    }
}
