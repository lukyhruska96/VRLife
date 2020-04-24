using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using VrLifeServer.Core;

namespace VrLifeServerTest.Core
{
    public class UserTest : IDisposable
    {
        public UserTest()
        {
            VrLifeServer.VrLifeServer.Init();
        }

        public void Dispose()
        {
            VrLifeServer.VrLifeServer.OnProcessExit(null, null);
        }

        [Fact]
        public void RegisterTest()
        {

        }
    }
}
