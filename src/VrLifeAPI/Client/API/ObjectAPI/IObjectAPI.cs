using System;
using UnityEngine;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.API.ObjectAPI
{
    public delegate void PlayerPointAtEventHandler(ulong userId, Ray ray);
    public delegate void PlayerSelectEventHandler(ulong userId, Ray ray);

    /// <summary>
    /// API pro objektové aplikace.
    /// </summary>
    public interface IObjectAPI
    {
        /// <summary>
        /// Vyhledávání shaderu na straně klienta.
        /// </summary>
        /// <param name="shaderPath">Cesta k shaderu.</param>
        /// <returns>Hledaný shader.</returns>
        Shader FindShader(string shaderPath);

        /// <summary>
        /// Cesta k výchozímu shaderu
        /// </summary>
        /// <returns>Cesta k shaderu.</returns>
        string DefaltShaderPath();

        /// <summary>
        /// Getter výchozího materiálu pro aplikace.
        /// </summary>
        /// <returns>Výchozí materiál.</returns>
        Material GetDefaultMaterial();
        
    }
}
