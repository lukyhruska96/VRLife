using UnityEngine;
using VrLifeAPI.Client.Core.Character;

namespace VrLifeAPI.Client.Core.Wrappers
{
    /// <summary>
    /// Interface postavy uživatele
    /// </summary>
    public interface IAvatar
    {

        /// <summary>
        /// Getter ID uživatele ovládajícího danou postavu.
        /// </summary>
        /// <returns>ID uživatele.</returns>
        ulong GetUserId();

        /// <summary>
        /// Getter aktuálního stavu kostry.
        /// </summary>
        /// <returns>Stav kostry postavy.</returns>
        SkeletonState GetCurrentSkeleton();

        /// <summary>
        /// Nastavení nového stavu kostry postavy.
        /// </summary>
        /// <param name="skeleton">Stav kostry.</param>
        void SetSkeleton(SkeletonState skeleton);

        /// <summary>
        /// Getter hlavního herního objektu dané postavy.
        /// </summary>
        /// <returns>Instance herního objektu.</returns>
        UnityEngine.GameObject GetGameObject();

        /// <summary>
        /// Zapnutí ovládání postavy.
        /// </summary>
        /// <param name="enabled">Stav ovládání.</param>
        void SetControls(bool enabled);

        /// <summary>
        /// Získání herního objektu hlavy dané postavy.
        /// </summary>
        /// <returns>Instance herního objektu.</returns>
        UnityEngine.GameObject GetHead();

        /// <summary>
        /// Získání pole herních objektů celé kostry
        /// indexovaného podle hodnot SkeletonEnum
        /// </summary>
        /// <returns>pole instancí herních objektů.</returns>
        GameObject[] GetSkeletonParts();

        /// <summary>
        /// Getter skriptu ovládajícího přehrávaný zvuk
        /// z dané postavy.
        /// </summary>
        /// <returns>Instance přehrávače zvuku.</returns>
        ISoundPlayer GetSoundPlayer();

        /// <summary>
        /// Smazání celého objektu z místnosti.
        /// </summary>
        void Destroy();
    }
}
