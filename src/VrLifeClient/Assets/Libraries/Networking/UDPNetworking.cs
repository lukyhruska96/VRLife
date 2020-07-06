using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using VrLifeShared.Networking.Middlewares;

namespace VrLifeShared.Networking
{
    /// <summary>
    /// Protobuff message based UDP Server
    /// </summary>
    /// <typeparam name="T">Protobuff generated message type</typeparam>
    public class UDPSocketState<T> where T: IMessage<T>
    {
        public UdpClient Socket;
        public Func<T, T> MsgHandler;
        public MessageParser<T> MsgParser;
        public List<IMiddleware<T>> Middlewares;
    }

    public class UDPNetworking<T> : INetworking<T> where T: IMessage<T>, new()
    {
        private UdpClient socket;
        private UDPSocketState<T> socketState = new UDPSocketState<T>();
        private List<IMiddleware<T>> _middlewares;

        // Sending only instance
        public UDPNetworking(List<IMiddleware<T>> middlewares)
        {
            this._middlewares = middlewares;
        }

        // Instance with listenner
        public UDPNetworking(IPAddress ipAddress, int port, Func<T, T> msgHandler, List<IMiddleware<T>> middlewares)
        {
            this._middlewares = middlewares;
            IPEndPoint endpoint = new IPEndPoint(ipAddress, port);
            this.socket = new UdpClient(endpoint);
            this.socketState.Socket = socket;
            this.socketState.MsgHandler = msgHandler;
            this.socketState.MsgParser = new MessageParser<T>(() => new T());
            this.socketState.Middlewares = middlewares;
        }

        public void StartListening()
        {
            if (this.socket == null) {
                throw new MissingMethodException("This instance cannot listen.");
            }
            socket.BeginReceive(new AsyncCallback(OnUdpData), this.socketState);
        }

        private static void OnUdpData(IAsyncResult result)
        {
            UDPSocketState<T> state = result.AsyncState as UDPSocketState<T>;
            UdpClient socket = state.Socket;
            IPEndPoint source = new IPEndPoint(0, 0);
            byte[] message = socket.EndReceive(result, ref source);

            // listen for next request
            socket.BeginReceive(new AsyncCallback(OnUdpData), state);

            //handle received message and send response
            T msg = state.MsgParser.ParseFrom(message);
            foreach (IMiddleware<T> middleware in state.Middlewares) {
                msg = middleware.TransformInputMsg(msg);
            }
            T response = state.MsgHandler(msg);
            byte[] rawResponse = response.ToByteArray();
            socket.Send(rawResponse, rawResponse.Length, source);
        }

        public void SendAsync(T req, IPEndPoint address, Action<T> callback, Action<Exception> err = null)
        {
            Task.Run(() =>
            {
            try
            {
                    callback(Send(req, address));
                }
                catch(Exception e)
                {
                    err?.Invoke(e);
                }
            });
        }

        public T Send(T req, IPEndPoint address)
        {
            foreach (IMiddleware<T> middleware in this._middlewares)
            {
                req = middleware.TransformOutputMsg(req);
            }
            UdpClient socket = new UdpClient();
            socket.Client.ReceiveTimeout = 5000;
            socket.Client.SendTimeout = 5000;
            byte[] data = req.ToByteArray();
            socket.Send(data, data.Length, address);
            byte[] response = socket.Receive(ref address);
            MessageParser<T> parser = new MessageParser<T>(() => new T());
            socket.Close();
            T parsedResponse = parser.ParseFrom(response);
            foreach (IMiddleware<T> middleware in _middlewares)
            {
                parsedResponse = middleware.TransformInputMsg(parsedResponse);
            }
            return parsedResponse;
        }
    }
}
