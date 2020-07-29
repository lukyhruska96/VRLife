using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.RoomService;
using VrLifeAPI.Provider.Core.Services;

namespace VrLifeAPI.Provider.Core.Services.RoomService
{
    /// <summary>
    /// Interface služby místností na straně Providera.
    /// </summary>
    public interface IRoomServiceProvider : IRoomService, IServiceProvider
    {

        /// <summary>
        /// Převod ID uživatele na ID místnosti v které se nachází.
        /// 
        /// null v případě, že uživatel není online.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <returns>ID místnosti.</returns>
        uint? RoomIdByUserId(ulong userId);
    }
}
