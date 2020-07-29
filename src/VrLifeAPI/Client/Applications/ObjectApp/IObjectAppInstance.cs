using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VrLifeAPI.Client.Applications.ObjectApp
{
    /// <summary>
    /// Interface instance objektové aplikace.
    /// </summary>
    public interface IObjectAppInstance : IDisposable
    {
        /// <summary>
        /// Metoda volaná PlayerControllerem v případě, že se
        /// daný hráč nachází uvnitř vymezeného prostoru instance
        /// objektové aplikace a ukazuje na objekt uvnitř tohoto prostoru.
        /// 
        /// Volání obdrží aplikace pouze od uživatele přihlášeného v daném
        /// klientovi.
        /// 
        /// V případě interakce mezi více hráči je nutné mít Forwarder instanci
        /// aplikace pro přeposílání událostí v aplikaci.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <param name="ray">Vektor směru s pozicí zdroje ve formě Ray objektu.</param>
        void PlayerPointAt(ulong userId, Ray ray);

        /// <summary>
        /// Metoda volaná PlayerControllerem v případě, že uživatel,
        /// který se nachází aktuálně ve vymezeném prostoru dané
        /// instance aplikace, vyvolal akci Select nad objektem nacházejícího
        /// také v daném prostoru.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <param name="ray">Vektor směru s pozicí zdroje ve formě Ray objektu.</param>
        void PlayerSelect(ulong userId, Ray ray);

        /// <summary>
        /// Getter herního hlavního herního objektu aplikace.
        /// </summary>
        /// <returns>Instance GameObject.</returns>
        GameObject GetGameObject();

        /// <summary>
        /// Metoda volaná po vložení objektu na vymezené místo v prostoru
        /// mísnosti, kdy může dojít k narušení transform komponenty,
        /// která byla nastavena vzhledem k předchozí lokaci.
        /// </summary>
        void FixGameObject();

        /// <summary>
        /// Getter ObjectAppInfo objektu popisující objektovou aplikaci,
        /// které je tento objekt instancí v místnosti.
        /// </summary>
        /// <returns>Instance ObjectAppInfo.</returns>
        ObjectAppInfo GetObjectAppInfo();

        /// <summary>
        /// Getter vybraného středu v prostoru místnosti dané instance.
        /// </summary>
        /// <returns>Vektor pozice středu vymezeného prostoru v místnosti.</returns>
        Vector3 GetCenter();
    }
}
