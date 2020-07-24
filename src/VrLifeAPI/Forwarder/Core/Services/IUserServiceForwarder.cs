
using VrLifeAPI.Common.Core.Services.UserService;

namespace VrLifeAPI.Forwarder.Core.Services.UserService
{
    public interface IUserServiceForwarder : IUserService, IServiceForwarder
    {
        IUser GetUserById(ulong userId, bool cached = false);
        ulong? GetUserIdByClientId(ulong clientId, bool cached = false);
        bool FastCheckUserId(ulong clientId, ulong userId);
    }
}
