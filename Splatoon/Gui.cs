using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Splatoon
{
    class Gui : IDisposable
    {
        readonly Splatoon p;
        public Gui(Splatoon p)
        {
            this.p = p;
            p._pi.UiBuilder.OnBuildUi += Draw;
        }

        public void Dispose()
        {
            p._pi.UiBuilder.OnBuildUi -= Draw;
        }

        void Draw()
        {
            if (p._pi.ClientState == null || p._pi.ClientState.LocalPlayer == null) return;
            var t = p._pi.ClientState.Targets.CurrentTarget;
            if (t != null && t is BattleNpc)
            {
                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(0, 0));
                ImGui.Begin("Splatoon ring", ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.NoTitleBar
                    | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoBackground);
                ImGui.SetWindowPos(new Num.Vector2(0, 0));
                ImGui.SetWindowSize(ImGui.GetIO().DisplaySize);
                DrawPoint(t.Position.X, t.Position.Y, t.Position.Z, 0xff0000ff);
                ImGui.End();
                ImGui.PopStyleVar();
            }
        }

        public void DrawRingWorld(float x, float y, float z, float radius, int num_segments, float thicc, uint colour)
        {
            int seg = num_segments / 2;

            for (int i = 0; i <= num_segments; i++)
            {
                p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x + (radius * (float)Math.Sin((Math.PI / seg) * i)), z, y + (radius * (float)Math.Cos((Math.PI / seg) * i))), out SharpDX.Vector2 pos);
                ImGui.GetWindowDrawList().PathLineTo(new System.Numerics.Vector2(pos.X, pos.Y));
            }
            ImGui.GetWindowDrawList().PathStroke(colour, true, thicc);
        }

        public void DrawPoint(float x, float y, float z, uint col)
        {
            p._pi.Framework.Gui.WorldToScreen(new SharpDX.Vector3(x, z, y), out SharpDX.Vector2 pos);
            ImGui.GetWindowDrawList().AddCircleFilled(
                new Num.Vector2(pos.X, pos.Y),
                2f,
                ImGui.GetColorU32(col),
                100);
        }
    }
}
