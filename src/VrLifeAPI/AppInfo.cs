

using System;

namespace VrLifeAPI
{
    public enum AppType
    {
        APP_BACKGROUND,
        APP_MENU,
        APP_OBJECT,
        APP_GLOBAL
    }

    public class AppInfo
    {
        public AppInfo(ulong id, string name, string desc, AppVersion version, AppType type, AppDependency[] dependencies = null)
        {
            this.ID = id;
            this.Name = name;
            this.Description = desc;
            this.Version = version;
            this.Type = type;
            this.Dependencies = dependencies;
        }

        public ulong ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public AppVersion Version { get; private set; }
        public AppType Type { get; private set; }
        public AppDependency[] Dependencies { get; private set; }

    }

    public struct AppDependency
    {
        public ulong ID { get; private set; }
        public AppVersion MinVersion { get; private set; }

        public AppDependency(ulong id, AppVersion minVersion)
        {
            ID = id;
            MinVersion = minVersion;
        }
    }

    public struct AppVersion : IComparable<AppVersion>
    {
        private int[] _version;
        public AppVersion(int[] version)
        {
            if(version == null || version.Length != 3)
            {
                throw new FormatException("Invalid version format.");
            }
            _version = version;
        }

        public override string ToString()
        {
            if(_version == null)
            {
                return "?.?.?";
            }
            return $"{_version[0]}.{_version[1]}.{_version[2]}";
        }

        public int[] ToNumeric()
        {
            return _version;
        }

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
