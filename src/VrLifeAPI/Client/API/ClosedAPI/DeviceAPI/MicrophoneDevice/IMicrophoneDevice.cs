using System;

namespace VrLifeAPI.Client.API.ClosedAPI.DeviceAPI.MicrophoneDevice 
{
    /// <summary>
    /// Poskytuje přístup ke všem přístupným záznamovým zařízením.
    /// Přístup k tomuto objektu je možný pouze pomocí <c>ClosedAPI</c>.
    /// </summary>
    public interface IMicrophoneDevice : IDisposable
    {
        /// <summary>
        /// Frekvence záznamu z aktuálně zvoleného zařízení.
        /// </summary>
        int Frequency { get; }

        /// <summary>
        /// Počet vzorků ve vzorkovacím segmentu.
        /// </summary>
        int SampleLength { get; }
        /// <summary>
        /// Reálná časová délka každého vzorkovacího segmentu v ms.
        /// </summary>
        int SampleDurationMS { get; }

        /// <summary>
        /// Event vyvolaný pro každý zaznamenaný vzorkovací segment o délce <c>SampleLength</c>
        /// </summary>
        /// <param name="ulong">ID záznamu</param>
        /// <param name="float[]">Pole jednotlivých vzorků</param>
        event Action<ulong, float[]> MicrophoneData;
        
        /// <summary>
        /// Ztlumení mikrofonu dle specifikované hodnoty parametru.
        /// </summary>
        /// <param name="state">Zvolený stav mikrofonu</param>
        void SetMute(bool state);

        /// <summary>
        /// Seznam dostupných záznamových zařízení.
        /// </summary>
        /// <returns>Seznam názvů daných zařízení</returns>
        string[] GetDevices();

        /// <summary>
        /// Volba zařízení pro záznam.
        /// </summary>
        /// <param name="idx">Index zařízení ze seznamu <c>GetDevices</c></param>
        void SetMic(int idx);
    }
}
