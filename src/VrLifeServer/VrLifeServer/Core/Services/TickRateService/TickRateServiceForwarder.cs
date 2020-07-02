using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VrLifeServer.API.Forwarder;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRateServiceForwarder : ITickRateServiceForwarder
    {
        private ConcurrentDictionary<uint, ConcurrentDictionary<ulong, TickRoom>>
        public MainMessage HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            InitTicker();
        }

        public bool IsActive(ulong userId, uint roomId)
        {
            return true;
        }

        private void InitTicker()
        {
            Task ticker = new Task(this.Ticker, TaskCreationOptions.LongRunning);
            ticker.Start();
        }

        private void Ticker()
        {

        }
    }
}
