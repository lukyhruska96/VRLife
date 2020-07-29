using Assets.Scripts.API.HUDAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeClient.API.HUDAPI
{
    /// <summary>
    /// API pro komunikaci s HUD rozhraním (pro Menu aplikace)
    /// </summary>
    public interface IHUDAPI
    {
        /// <summary>
        /// Zobrazí notifikace v pravém dolním rohu HUD rozhraní po dobu 5 vteřin.
        /// </summary>
        /// <param name="notification">Notifikace k zobrazení.</param>
        void ShowNotification(Notification notification);
    }
}
