using UnityEngine;


namespace VrLifeAPI.Client.Core.Character
{
    /// <summary>
    /// Interface skriptu ovládajícího zvuk přehrávaný
    /// z dané postavy.
    /// </summary>
    public interface ISoundPlayer
    {
        /// <summary>
        /// Přehrání zvukové stopy.
        /// </summary>
        /// <param name="clip">Zvuková stopa k přehrání.</param>
        void Play(AudioClip clip);
    }
}
