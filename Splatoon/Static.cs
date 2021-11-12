global using Dalamud.Game.ClientState.Objects.Types;
global using System.Globalization;
global using System.IO;
global using System.IO.Compression;
global using System.Numerics;
global using Dalamud.Plugin;
global using ImGuiNET;
global using System.Runtime.ExceptionServices;
global using System.Collections.Concurrent;
global using Dalamud.Logging;
global using static Splatoon.Static;
global using Dalamud.Interface;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
using System.Diagnostics;

namespace Splatoon
{
    static class Static
    {
        public static void Toggle<T>(this HashSet<T> h, T o)
        {
            if(h.Contains(o))
            {
                h.Remove(o);
            }
            else
            {
                h.Add(o);
            }
        }

        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool StartsWithIgnoreCase(this string a, string b)
        {
            return a.StartsWith(b, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string a, string b)
        {
            return a.Contains(b, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string Compress(this string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionLevel.Optimal))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray()).Replace('+', '-').Replace('/', '_');
            }
        }

        public static string ToBase64UrlSafe(this string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace('+', '-').Replace('/', '_');
        }

        public static string FromBase64UrlSafe(this string s)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(s.Replace('-', '+').Replace('_', '/')));
        }

        public static string Decompress(this string s)
        {
            var bytes = Convert.FromBase64String(s.Replace('-', '+').Replace('_', '/'));
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }



        //because Dalamud changed Y and Z in actor positions I have to do emulate old behavior to not break old presets
        public static Vector3 GetPlayerPositionXZY()
        {
            if (Svc.ClientState.LocalPlayer != null)
            {
                if (Splatoon.PlayerPosCache == null)
                {
                    Splatoon.PlayerPosCache = new Vector3(
                        Svc.ClientState.LocalPlayer.Position.X,
                     Svc.ClientState.LocalPlayer.Position.Z,
                     Svc.ClientState.LocalPlayer.Position.Y);
                }
                return Splatoon.PlayerPosCache.Value;
            }
            return Vector3.Zero;
        }

        public static Vector3 GetPositionXZY(this GameObject a)
        {
            return new Vector3(a.Position.X,
                    a.Position.Z,
                    a.Position.Y);
        }

        public static Vector4 ToVector4(this uint col)
        {
            return ImGui.ColorConvertU32ToFloat4(col);
        }

        public static void ProcessStart(string s)
        {
            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = s
                });
            }
            catch (Exception e)
            {
                Svc.Chat.Print("Error: " + e.Message + "\n" + e.StackTrace);
            }
        }

        public static string NotNull(this string s)
        {
            return s ?? "";
        }
        public static float AngleBetweenVectors(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            return (float)Math.Acos(((x2 - x1) * (x4 - x3) + (y2 - y1) * (y4 - y3)) /
                (Math.Sqrt(Square(x2 - x1) + Square(y2 - y1)) * Math.Sqrt(Square(x4 - x3) + Square(y4 - y3))));
        }

        public static float Square(float x)
        {
            return x * x;
        }
    
        public static float RadToDeg(float radian)
        {
            return (radian * (180 / Splatoon.FloatPI));
        }

        public static void Safe(Action a)
        {
            try
            {
                a();
            }
            catch(Exception e)
            {
                PluginLog.Error($"{e.Message}\n{e.StackTrace ?? ""}");
            }
        }
        public static Vector3 RotatePoint(float cx, float cy, float angle, Vector3 p)
        {
            if (angle == 0f) return p;
            var s = (float)Math.Sin(angle);
            var c = (float)Math.Cos(angle);

            // translate point back to origin:
            p.X -= cx;
            p.Y -= cy;

            // rotate point
            float xnew = p.X * c - p.Y * s;
            float ynew = p.X * s + p.Y * c;

            // translate point back:
            p.X = xnew + cx;
            p.Y = ynew + cy;
            return p;
        }
    }
}
