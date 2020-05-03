using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Networking.Middlewares
{
    public interface IMiddleware<T>
    {
        T TransformInputMsg(T msg);
        T TransformOutputMsg(T msg);
    }
}
