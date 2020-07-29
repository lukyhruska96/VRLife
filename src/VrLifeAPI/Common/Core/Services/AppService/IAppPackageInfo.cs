using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.AppService
{
    /// <summary>
    /// Interface informace o balíčku aplikace.
    /// </summary>
    public interface IAppPackageInfo
    {
        /// <summary>
        /// ID aplikace
        /// </summary>
        ulong ID { get; }

        /// <summary>
        /// Název aplikace
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Popis aplikace
        /// </summary>
        string Desc { get; }

        /// <summary>
        /// Typ aplikace.
        /// </summary>
        AppType Type { get; }

        /// <summary>
        /// Verze aplikace
        /// </summary>
        AppVersion Version { get; }

        /// <summary>
        /// Informace o instanci balíčku aplikace na straně providera.
        /// 
        /// V případě null hodnoty, neexistuje instance aplikace na straně providera.
        /// </summary>
        IAppPackageItemInfo Provider { get; }

        /// <summary>
        /// Informace o instanci balíčku aplikace na straně forwardera.
        /// 
        /// V případě null hodnoty, neexistuje instance aplikace na straně forwardera.
        /// </summary>
        IAppPackageItemInfo Forwarder { get; }

        /// <summary>
        /// Informace o instanci balíčku aplikace na straně klienta.
        /// </summary>
        IAppPackageItemInfo Client { get; }

        /// <summary>
        /// Závislost na dalších balíčcích s jejich minimální požadovanou verzí.
        /// 
        /// Pokud aplikace poskytuje vlastní API pro komunikaci s ostatními aplikacemi,
        /// musí být vždy nové verze zpětně kompatibilní.
        /// </summary>
        IAppPackageInfo[] Dependencies { get; }

        /// <summary>
        /// Převod informace o balíčku aplikace na informace o aplikaci.
        /// </summary>
        /// <returns>Instance AppInfo</returns>
        AppInfo ToAppInfo();

        /// <summary>
        /// Převod na síťovací model k odeslání.
        /// </summary>
        /// <returns>Síťovací model.</returns>
        AppPackageInfoMsg ToNetworkModel();

        /// <summary>
        /// Převod na síťovací model pro seznam balíčků k odeslání.
        /// </summary>
        /// <returns>Síťovací model.</returns>
        AppPackageListEl ToNetworkListEl();

        /// <summary>
        /// Převod na síťový model závislosti k odeslání.
        /// </summary>
        /// <returns>Síťovací model.</returns>
        AppPackageDependency ToNetworkDependency();
    }

    /// <summary>
    /// Informace o instanci balíčku pro daný typ zařízení.
    /// </summary>
    public interface IAppPackageItemInfo
    {
        /// <summary>
        /// Cesta k dll souboru.
        /// </summary>
        string DLLPath { get; }

        /// <summary>
        /// Cesta k .zip souboru.
        /// </summary>
        string ZipPath { get; }
    }
}
