using System;

namespace VrLifeAPI
{

    /// <summary>
    /// Enum typu aplikace
    /// </summary>
    public enum AppType
    {
        APP_BACKGROUND,
        APP_MENU,
        APP_OBJECT,
        APP_GLOBAL
    }


    /// <summary>
    /// Informace o aplikaci.
    /// </summary>
    public class AppInfo
    {

        /// <summary>
        /// Výchozí konstruktor.
        /// </summary>
        /// <param name="id">ID aplikace.</param>
        /// <param name="name">Název aplikace.</param>
        /// <param name="desc">Popis aplikace.</param>
        /// <param name="version">Verze aplikace.</param>
        /// <param name="type">Typ aplikace.</param>
        /// <param name="dependencies">Závislost na jiných aplikacích.</param>
        public AppInfo(ulong id, string name, string desc, AppVersion version, AppType type, AppDependency[] dependencies = null)
        {
            this.ID = id;
            this.Name = name;
            this.Description = desc;
            this.Version = version;
            this.Type = type;
            this.Dependencies = dependencies;
        }

        /// <summary>
        /// ID aplikace.
        /// </summary>
        public ulong ID { get; private set; }

        /// <summary>
        /// Název aplikace.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Popis aplikace.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Verze aplikace.
        /// </summary>
        public AppVersion Version { get; private set; }

        /// <summary>
        /// Typ aplikace.
        /// </summary>
        public AppType Type { get; private set; }

        /// <summary>
        /// Závislost na jiných aplikacích.
        /// </summary>
        public AppDependency[] Dependencies { get; private set; }

    }

    /// <summary>
    /// Závislost na jiných aplikacích.
    /// </summary>
    public struct AppDependency
    {
        /// <summary>
        /// ID aplikace.
        /// </summary>
        public ulong ID { get; private set; }

        /// <summary>
        /// Minimální požadovaná verze aplikace.
        /// </summary>
        public AppVersion MinVersion { get; private set; }

        public AppDependency(ulong id, AppVersion minVersion)
        {
            ID = id;
            MinVersion = minVersion;
        }
    }

    /// <summary>
    /// Verze aplikace.
    /// </summary>
    public struct AppVersion : IComparable<AppVersion>
    {
        private int[] _version;

        /// <summary>
        /// Konstruktor verze aplikace.
        /// </summary>
        /// <param name="version">verze ve formě int array o velikosti 3 čísel.</param>
        public AppVersion(int[] version)
        {
            if(version == null || version.Length != 3)
            {
                throw new FormatException("Invalid version format.");
            }
            _version = version;
        }

        /// <summary>
        /// Převod verze na textovou reprezentaci.
        /// </summary>
        /// <returns>Textová reprezentace verze.</returns>
        public override string ToString()
        {
            if(_version == null)
            {
                return "?.?.?";
            }
            return $"{_version[0]}.{_version[1]}.{_version[2]}";
        }

        /// <summary>
        /// Převod na verzi int pole.
        /// </summary>
        /// <returns>Interpretace verze pomocí int pole.</returns>
        public int[] ToNumeric()
        {
            return _version;
        }

        /// <summary>
        /// Porovnávání verzí.
        /// </summary>
        /// <param name="other">Druhá verze k porovnání.</param>
        /// <returns>Porovnávací výsledná hodnota.</returns>
        public int CompareTo(AppVersion other)
        {
            for(int i = 0; i < 3; ++i)
            {
                int val = _version[i] - other._version[i];
                if(val != 0)
                {
                    return val;
                }
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is AppVersion && (AppVersion)obj == this;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator <=(AppVersion o1, AppVersion o2)
        {
            return o1.CompareTo(o2) <= 0;
        }
        public static bool operator >=(AppVersion o1, AppVersion o2)
        {
            return o1.CompareTo(o2) >= 0;
        }

        public static bool operator <(AppVersion o1, AppVersion o2)
        {
            return o1.CompareTo(o2) < 0;
        }

        public static bool operator>(AppVersion o1, AppVersion o2)
        {
            return o1.CompareTo(o2) > 0;
        }

        public static bool operator==(AppVersion o1, AppVersion o2)
        {
            return o1.CompareTo(o2) == 0;
        }
        public static bool operator !=(AppVersion o1, AppVersion o2)
        {
            return o1.CompareTo(o2) != 0;
        }
    }
}
