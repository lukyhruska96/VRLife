using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// Enum příjemce zprávy.
    /// </summary>
    public enum EventRecipient
    {
        FORWARDER,
        PROVIDER
    }

    /// <summary>
    /// Interface služby událostí.
    /// </summary>
    public interface IEventServiceClient : IServiceClient
    {
        /// <summary>
        /// Poslední naměřený Road Trip Time v ms.
        /// </summary>
        uint LastRTT { get; }

        /// <summary>
        /// Odeslání nového stavu kostry postavy přihlášeného uživatele.
        /// </summary>
        /// <param name="skeleton">Kostra k odeslání.</param>
        /// <returns>ServiceCallback s návratovou hodnotou byte array, která by měla být null.</returns>
        IServiceCallback<byte[]> SendSkeleton(SkeletonState skeleton);

        /// <summary>
        /// Odeslání vlastní události.
        /// </summary>
        /// <param name="eventData">Data událostí v objektu ke komunikaci po síti.</param>
        /// <param name="recipient">Příjemce události.</param>
        /// <returns>ServiceCallback s návratovou hodnotou typu byte array z pole 'data' v odpovědi.</returns>
        IServiceCallback<byte[]> SendEvent(EventDataMsg eventData, EventRecipient recipient);
    }
}
