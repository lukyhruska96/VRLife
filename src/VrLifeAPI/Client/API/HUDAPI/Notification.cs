using VrLifeAPI;

namespace Assets.Scripts.API.HUDAPI
{
    /// <summary>
    /// Objekt notifikace k zobrazení v HUD rozhraní.
    /// </summary>
    public struct Notification
    {
        /// <summary>
        /// Titulek notifikace.
        /// </summary>
        public string Header { get; private set; }

        /// <summary>
        /// Identifikační číslo aplikace, která zobrazuje danou notifikace.
        /// ID je poté použito při kliknutí na notifikaci pro přesměrování v menu.
        /// </summary>
        public ulong AppId { get; private set; }
        /// <summary>
        /// Text dané notifikace.
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// Speciální označení notifikace pro přesměrování uvnitř dané menu aplikace.
        /// Po přesměrování na aplikaci podle AppId je zavolána metoda HandleNotification,
        /// která podle ActionPath může zobrazit detailnější informace o zprávě z notifikace.
        /// </summary>
        public string ActionPath { get; private set; }

        /// <summary>
        /// Konstruktor notifikace.
        /// </summary>
        /// <param name="appInfo">AppInfo dané aplikce.</param>
        /// <param name="text">Zobrazovaný text.</param>
        /// <param name="actionPath">Specilní označení notifikace pro upřesnění přesměrování.</param>
        public Notification(AppInfo appInfo, string text, string actionPath = null)
        {
            Header = appInfo.Name;
            AppId = appInfo.ID;
            Text = text;
            ActionPath = actionPath;
        }
    }
}
