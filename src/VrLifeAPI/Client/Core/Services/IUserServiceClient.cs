using System;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// Interface uživatelské služby.
    /// </summary>
    public interface IUserServiceClient : IServiceClient
    {
        /// <summary>
        /// ID aktuálně přihlášeného uživatele.
        /// 
        /// null v případě, že uživatel není přihlášen.
        /// </summary>
        ulong? UserId { get; }

        /// <summary>
        /// Event vyvolaný v případě odhlášení uživatele.
        /// </summary>
        event Action UserLoggedOut;

        /// <summary>
        /// Získání seznamu uživatelů nacházejících se v aktuální místnosti.
        /// </summary>
        /// <returns>ServiceCallback s návratovou hodnotou síťovacího objektu seznamu uživatelů.</returns>
        IServiceCallback<UserListMsg> CurrentRoomUsers();

        /// <summary>
        /// Žádost o přihlášení uživatele.
        /// </summary>
        /// <param name="username">Uživatelské jméno.</param>
        /// <param name="password">Uživatelské heslo.</param>
        /// <returns>ServiceCallback bez návratové hodnoty.</returns>
        IServiceCallback<bool> Login(string username, string password);


        /// <summary>
        /// Žádost o registraci nového uživatele.
        /// </summary>
        /// <param name="username">Uživatelské jméno.</param>
        /// <param name="password">Uživatelské heslo.</param>
        /// <returns>ServiceCallback bez návratové hodnoty.</returns>
        IServiceCallback<bool> Register(string username, string password);

        /// <summary>
        /// Reset logiky služby v případě odhlášení uživatele.
        /// </summary>
        void Reset();

        /// <summary>
        /// Vyvolání události UserLoggedOut
        /// </summary>
        void OnUserLogout();
    }
}
