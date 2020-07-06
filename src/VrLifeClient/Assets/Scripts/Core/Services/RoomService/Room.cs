using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.RoomService
{
    public class Room
    {
        private const uint DEFAULT_TICKRATE = 32;
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint Capacity { get; set; }

        // list of user IDs
        public List<ulong> Players { get; } = new List<ulong>();
        public IPEndPoint Address { get; set; }
        public uint TickRate { get; set; }
        public ulong StartTime { get; set; }

        public Room()
        {
            this.TickRate = DEFAULT_TICKRATE;
            this.StartTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public bool IsFull()
        {
            return Players.Count >= Capacity;
        }

        public bool IsEmpty()
        {
            return Players.Count <= 0;
        }

        public Room(RoomDetail roomDetail)
        {
            this.Id = roomDetail.RoomId;
            this.Name = roomDetail.Name;
            this.Capacity = roomDetail.Capacity;
            this.Players.AddRange(roomDetail.Players);
            this.Address = new IPEndPoint(roomDetail.ServerAddress, roomDetail.Port);
            this.TickRate = roomDetail.TickRate;
            this.StartTime = roomDetail.StartTime;
        }

        public RoomDetail ToNetworkModel()
        {
            RoomDetail roomDetail = new RoomDetail();
            roomDetail.RoomId = this.Id;
            roomDetail.Name = this.Name;
            roomDetail.Capacity = this.Capacity;
            roomDetail.Players.AddRange(this.Players);
            roomDetail.ServerAddress = Address.Address.ToInt();
            roomDetail.Port = Address.Port;
            return roomDetail;
        }
    }
}
