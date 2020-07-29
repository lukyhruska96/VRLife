using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.UserService;

namespace VrLifeAPI.Provider.Core.Services.UserService
{
    /// <summary>
    /// Interface uživatelské služby na straně Providera.
    /// </summary>
    public interface IUserServiceProvider : IUserService, IServiceProvider
    {

        /// <summary>
        /// Získání objektu uživatele podle klientského ID.
        /// 
        /// null v případě, že žádný uživatel není přihlášený na daném klientovi.
        /// </summary>
        /// <param name="clientId">ID klienta.</param>
        /// <returns>Objekt uživatele.</returns>
        IUser GetUserByClientId(ulong clientId);
    }
}
