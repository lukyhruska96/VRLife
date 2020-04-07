using System;
namespace VrLifeServer.Applications
{
    public interface IApplication
    {
        int Init();
        void Run();
        void End();
        void Update();
    }
}
