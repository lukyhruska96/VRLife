using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.UserService
{
    public interface IUser
    {

        ulong Id { get; }
        string Username { get; }

        bool CheckPassword(string password);

        void ChangePassword(string oldPassword, string newPassword);

        void Delete();

        UserDetailMsg ToMessage();
    }
}
