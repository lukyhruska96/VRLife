using Assets.Scripts.API.HUDAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeClient.API.HUDAPI
{
    public interface IHUDAPI
    {
        void ShowNotification(Notification notification);
    }
}
