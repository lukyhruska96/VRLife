using VrLifeAPI.Client.Services;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API.OpenAPI
{
    public interface IUserAPI
    {
        ulong? UserId { get; }

        IServiceCallback<bool> Register(string username, string password);

        IServiceCallback<bool> Login(string username, string password);

        IServiceCallback<UserListMsg> CurrentRoomUsers();

        void Logout();
    }
}
