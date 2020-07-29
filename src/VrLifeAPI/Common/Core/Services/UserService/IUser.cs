using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.UserService
{
    /// <summary>
    /// Interface objektu uživateli
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// ID uživatele
        /// </summary>
        ulong Id { get; }

        /// <summary>
        /// Uživatelské jméno
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Kontrola správnosti hesla u daného užiatele.
        /// </summary>
        /// <param name="password">Zadané heslo ke kontrole.</param>
        /// <returns>true - správné, false - špatné</returns>
        bool CheckPassword(string password);

        /// <summary>
        /// Změna hesla uživatele.
        /// </summary>
        /// <param name="oldPassword">Staré heslo.</param>
        /// <param name="newPassword">Nové heslo.</param>
        void ChangePassword(string oldPassword, string newPassword);

        /// <summary>
        /// Smazání uživatele.
        /// </summary>
        void Delete();

        /// <summary>
        /// Převod na síťovací objekt k odeslání.
        /// </summary>
        /// <returns>Síťovací objekt.</returns>
        UserDetailMsg ToMessage();
    }
}
