using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using ECommons;
using ECommons.DalamudServices;
using ECommons.ExcelServices.TerritoryEnumeration;
using ECommons.GameFunctions;
using ECommons.Logging;
using ECommons.MathHelpers;
using Splatoon.SplatoonScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SplatoonScriptsOfficial.Tests
{
    public class GenericTest : SplatoonScript
    {
        public override HashSet<uint> ValidTerritories => new() {  };

        public override void OnSetup()
        {
            this.Controller.TryRegisterElement("test", new(1) { radius = 1, refActorComparisonType = 2, includeRotation = true});
        }

        public override void OnUpdate()
        {
            if (this.Controller.TryGetElementByName("test", out var e))
            {
                if (Svc.ClientState.LocalPlayer != null)
                {
                    e.refActorObjectID = Svc.ClientState.LocalPlayer.ObjectId;
                    e.AdditionalRotation = (MathF.PI / 180) * 55;
                    e.offY = 10;

                }
            }
        }

        Vector3 GetRotatedPointOffsetToObject(GameObject obj, float angle, float offset)
        {
            return GetRotatedPoint(obj.Position, obj.Rotation - (MathF.PI / 180) * angle, offset);
        }

        Vector3 GetRotatedPoint(Vector3 originPosition, float angle, float distance)
        {
            return RotatePoint(originPosition.X, originPosition.Z,
                -angle, new Vector3(
                originPosition.X,
                originPosition.Y,
                originPosition.Z + distance));
        }

        Vector3 RotatePoint(float cx, float cy, float angle, Vector3 p)
        {
            if (angle == 0f) return p;
            var s = (float)Math.Sin(angle);
            var c = (float)Math.Cos(angle);

            // translate point back to origin:
            p.X -= cx;
            p.Z -= cy;

            // rotate point
            float xnew = p.X * c - p.Z * s;
            float ynew = p.X * s + p.Z * c;

            // translate point back:
            p.X = xnew + cx;
            p.Z = ynew + cy;
            return p;
        }
    }
}
