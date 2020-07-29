using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.TickRateService;

namespace VrLifeAPI.Forwarder.Core.Services.TickRateService
{
    /// <summary>
    /// Interface TickRate služby na straně Forwardera.
    /// </summary>
    public interface ITickRateServiceForwarder : ITickRateService, IServiceForwarder
    {
        /// <summary>
        /// Kontrola, zda je daný uživatel v místnosti aktivní.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <param name="roomId">ID místnosti.</param>
        /// <returns>true - aktivní, false - neaktivní</returns>
        bool IsActive(ulong userId, uint roomId);

        /// <summary>
        /// Nastavení stavu kostry daného uživatele v místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <param name="userId">ID uživatele.</param>
        /// <param name="skeleton">Nová kostra.</param>
        void SetSkeletonState(uint roomId, ulong userId, SkeletonState skeleton);

        /// <summary>
        /// Nastavení stavu objektu v místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <param name="appId">ID aplikace.</param>
        /// <param name="appInstanceId">ID instance dané aplikace.</param>
        /// <param name="obj">Nový stav objektu.</param>
        void SetObjectState(uint roomId, ulong appId, ulong appInstanceId, ObjectState obj);
    }
}
