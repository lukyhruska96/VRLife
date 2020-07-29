using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeAPI.Client.Applications.ObjectApp
{
    /// <summary>
    /// Informace o objektové aplikaci
    /// </summary>
    public class ObjectAppInfo
    {
        /// <summary>
        /// Šířka vyžadovaného prostoru pro instanci aplikace.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Délka vyžadovaného prostoru pro instanci aplikace.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Konstruktor daného objektu
        /// </summary>
        /// <param name="width">Vyžadovaná šířka objektu.</param>
        /// <param name="height">Vyžadovaná délka objektu.</param>
        public ObjectAppInfo(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
