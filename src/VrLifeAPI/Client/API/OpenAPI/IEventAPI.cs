using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API.OpenAPI
{
    /// <summary>
    /// API pro komunikaci s EventService pomocí OpenAPI
    /// </summary>
    public interface IEventAPI
    {
        /// <summary>
        /// Odeslání stavu postavy lokálního uživatele.
        /// </summary>
        /// <param name="skeleton">Struktura hodnot postavy.</param>
        /// <returns>ServiceCallback s návratovou hodnotou byte array dle pole data z EventResponse zprávy.</returns>
        IServiceCallback<byte[]> SendSkeleton(SkeletonState skeleton);

        /// <summary>
        /// Odeslání libovolné události specifikovanému adresátovi.
        /// </summary>
        /// <param name="msg">Zpráva k odeslání.</param>
        /// <param name="recipient">Příjemce dané zprávy (Provider/Forwarder).</param>
        /// <returns>ServiceCallback s návratovou hodnotou byte array dle pole data z EventResponse zprávy.</returns>
        IServiceCallback<byte[]> SendEvent(EventDataMsg msg, EventRecipient recipient);
    }
}
