using System.Collections.ObjectModel;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.API.GlobalAPI
{
    public interface IPlayersAPI
    {

        ReadOnlyDictionary<ulong, IAvatar> GetAvatars();

        IAvatar GetMainAvatar();

        void Init(IAvatar mainAvatar);

        void AddAvatar(ulong userId, IAvatar avatar);

        void AddMainAvatar(IAvatar avatar);

        IAvatar GetAvatar(ulong userId);

        void DeleteAvatar(ulong userId);
    }
}
