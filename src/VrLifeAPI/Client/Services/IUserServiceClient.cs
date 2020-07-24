using System;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Services
{
    public interface IUserServiceClient : IServiceClient
    {
        ulong? UserId { get; }

        event Action UserLoggedOut;

        IServiceCallback<UserListMsg> CurrentRoomUsers();

        IServiceCallback<bool> Login(string username, string password);

        IServiceCallback<bool> Register(string username, string password);

        void Reset();

        void OnUserLogout();
    }
}
