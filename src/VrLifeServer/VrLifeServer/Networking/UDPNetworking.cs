using Google.Protobuf;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace VrLifeServer.Networking
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
    }

    public class UDPNetworking<T> : INetworking<T> where T: IMessage<T>, new()
    {
        private UdpClient socket;
        private UDPSocketState<T> socketState;

        public UDPNetworking(IPAddress ipAddress, int port, Func<T, T> msgHandler)
        {
            IPEndPoint endpoint = new IPEndPoint(ipAddress, port);
            this.socket = new UdpClient(endpoint);
            this.socketState.Socket = socket;
            this.socketState.MsgHandler = msgHandler;
            this.socketState.MsgParser = new MessageParser<T>(() => new T());
            socket.BeginReceive(new AsyncCallback(OnUdpData), this.socketState);
        }

        public void StartListening()
        {
            throw new NotImplementedException();
        }

        private static void OnUdpData(IAsyncResult result)
        {
            UDPSocketState<T> state = result.AsyncState as UDPSocketState<T>;
            UdpClient socket = state.Socket;
            IPEndPoint source = new IPEndPoint(0, 0);
            byte[] message = socket.EndReceive(result, ref source);

            // listen for next request
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);

            //handle received message and send response
            T msg = state.MsgParser.ParseFrom(message);
            T response = state.MsgHandler(msg);
            byte[] rawResponse = response.ToByteArray();
            socket.Send(rawResponse, rawResponse.Length, source);
        }

        public void Send(T req, IPEndPoint address, Action<T> callback, Action<Exception> err = null)
        {
            Task.Run(() =>
            {
                try
                {
                    UdpClient socket = new UdpClient(address);
                    socket.Client.ReceiveTimeout = 5000;
                    socket.Client.SendTimeout = 5000;
                    byte[] data = req.ToByteArray();
                    socket.Send(data, data.Length);
                    byte[] response = socket.Receive(ref address);
                    MessageParser<T> parser = new MessageParser<T>(() => new T());
                    T parsedResponse = parser.ParseFrom(response);
                    callback(parsedResponse);
                }
                catch(Exception e)
                {
                    err?.Invoke(e);
                }
            });
        }
    }
}
