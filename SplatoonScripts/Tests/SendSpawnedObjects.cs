using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Logging;
using ECommons;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.Schedulers;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SplatoonScriptsOfficial.Tests
{
    public unsafe class SendSpawnedObjects : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new();
        HttpClient Client;

        public override void OnEnable()
        {
            Client = new()
            {
                Timeout = TimeSpan.FromSeconds(3),
            };
        }

        public override void OnDisable()
        {
            Client?.Dispose();
        }

        public override void OnObjectCreation(nint newObjectPtr)
        {
            new TickScheduler(delegate
            {
                if(Svc.Objects.TryGetFirst(x => x.Address == newObjectPtr, out var obj))
                {
                    var chr = obj is Character ? (Character)obj: null;
                    var data = new Data(obj.ObjectId, obj.DataId, chr == null ? 0 : chr.Struct()->ModelCharaId, obj.Position, obj.Rotation);
                    Client?.GetAsync($"http://127.0.0.1:8080/?data={HttpUtility.UrlEncode(data.ToString())}");
                }
            });
        }

        [Serializable]
        public record struct Data
        {
            public uint ObjectID;
            public uint DataID;
            public int ModelID;
            public Vector3 Position;
            public float Angle;

            public Data(uint objectID, uint dataID, int modelID, Vector3 position, float angle)
            {
                ObjectID = objectID;
                DataID = dataID;
                ModelID = modelID;
                Position = position;
                Angle = angle;
            }

            public override string ToString()
            {
                return $"{ObjectID}|{DataID}|{ModelID}|{Position.X}/{Position.Y}/{Position.Z}|{Angle}";
            }
        }
    }
}
