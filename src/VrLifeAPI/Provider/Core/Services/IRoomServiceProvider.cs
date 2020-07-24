using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.RoomService;
using VrLifeAPI.Provider.Core.Services;

namespace VrLifeAPI.Provider.Core.Services.RoomService
{
    public interface IRoomServiceProvider : IRoomService, IServiceProvider
    {
        uint? RoomIdByUserId(ulong userId);
    }
}
