using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API.OpenAPI
{
    /// <summary>
    /// API pro komunikaci s UserAPI pomocí OpenAPI
    /// </summary>
    public interface IUserAPI
    {
        /// <summary>
        /// Uživatelské ID aktuálně přihlášeného uživatele.
        /// Null v případě, že uživatel ještě není přihlášený.
        /// </summary>
        ulong? UserId { get; }

        /// <summary>
        /// Dotaz pro registraci nového uživatele.
        /// </summary>
        /// <param name="username">Uživatelské jméno.</param>
        /// <param name="password">Heslo.</param>
        /// <returns>
        /// ServiceCallback bez návratové hodnoty. 
        /// V případě chyby v argumentech vrací Error, který vyvolá Exception.
        /// </returns>
        IServiceCallback<bool> Register(string username, string password);

        /// <summary>
        /// Dotaz pro přihlášení uživatele.
        /// </summary>
        /// <param name="username">Uživatelské jméno.</param>
        /// <param name="password">Heslo.</param>
        /// <returns>
        /// ServiceCallback bez návratové hodnoty.
        /// V případě špatných údajů vrací Error, který vyvolá Exception.
        /// </returns>
        IServiceCallback<bool> Login(string username, string password);

        /// <summary>
        /// Seznam uživatelů nacházejících se v aktuální místnosti.
        /// </summary>
        /// <returns>Seznam uživatelů.</returns>
        IServiceCallback<UserListMsg> CurrentRoomUsers();

        /// <summary>
        /// Dotaz k odhlášení uživatele.
        /// </summary>
        void Logout();
    }
}
