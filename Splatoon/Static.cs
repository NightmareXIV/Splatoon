using ECommons.MathHelpers;
using System.Diagnostics;

namespace Splatoon;

static class Static
{
    public static string Format(this uint num)
    {
        return P.Config.Hexadecimal ? $"0x{num:X}" : $"{num}";
    }

    public static string Format(this int num)
    {
        return P.Config.Hexadecimal ? $"0x{num:X}" : $"{num}";
    }

    public static string Format(this long num)
    {
        return P.Config.Hexadecimal ? $"0x{num:X}" : $"{num}";
    }

    public static float GetAdditionalRotation(this Element e, float cx, float cy, float angle)
    {
        if (!e.FaceMe) return e.AdditionalRotation + angle;
        return (e.AdditionalRotation.RadiansToDegrees() + MathHelper.GetRelativeAngle(new Vector2(cx, cy), Svc.ClientState.LocalPlayer.Position.ToVector2())).DegreesToRadians();
    }
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
        return a.Equals(b, StringComparison.OrdinalIgnoreCase);
    }

    public static bool StartsWithIgnoreCase(this string a, string b)
    {
        return a.StartsWith(b, StringComparison.OrdinalIgnoreCase);
    }

    public static bool ContainsIgnoreCase(this string a, string b)
    {
        return a.Contains(b, StringComparison.OrdinalIgnoreCase);
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
        return MathF.Acos(((x2 - x1) * (x4 - x3) + (y2 - y1) * (y4 - y3)) /
            (MathF.Sqrt(Square(x2 - x1) + Square(y2 - y1)) * MathF.Sqrt(Square(x4 - x3) + Square(y4 - y3))));
    }

    public static IEnumerable<(Vector2 v2, float angle)> GetPolygon(List<Vector2> coords)
    {
        var medium = new Vector2(coords.Average(x => x.X), coords.Average(x => x.Y));
        var array = coords.Select(x => x - medium).ToArray();
        Array.Sort(array, delegate (Vector2 a, Vector2 b)
        {
            var angleA = MathF.Atan2(a.Y, a.X);
            var angleB = MathF.Atan2(b.Y, b.X);
            if (angleA == angleB)
            {
                var radiusA = MathF.Sqrt((a.X * a.X) + (a.Y * a.Y));
                var radiusB = MathF.Sqrt((b.X * b.X) + (b.Y * b.Y));
                return radiusA > radiusB ? 1 : -1;
            }
            return angleA > angleB ? 1 : -1;
        });
        foreach (var x in array) yield return (x + medium, MathF.Atan2(x.Y, x.X));
    }

    public static float Square(float x)
    {
        return x * x;
    }

    public static float RadToDeg(float radian)
    {
        return (radian * (180 / MathF.PI));
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

    // Calculate the distance between
    // point pt and the segment p1 --> p2.
    public static Vector3 FindClosestPointOnLine(Vector3 P, Vector3 A, Vector3 B)
    {
        var D = Vector3.Normalize(B - A);
        var d = Vector3.Dot(P - A, D);
        return A + Vector3.Multiply(D, d);
    }
    public static float DegreesToRadians(this float val)
    {
        return (float)((Math.PI / 180) * val);
    }
    public static float RadiansToDegrees(this float radians)
    {
        return (float)((180 / Math.PI) * radians);
    }

    public static string RemoveSymbols(this string s, IEnumerable<string> deletions)
    {
        foreach (var r in deletions) s = s.Replace(r, "");
        return s;
    }

    public static string ToStringNullSup(this bool? b)
    {
        if (b == null) return "null";
        return b.Value.ToString();
    }

    public static void BubbleSort(ref Vector2[] v2array, Func<Vector2, Vector2, bool> Comparer)
    {
        Vector2 temp;
        int count = v2array.Length;
        for (int outer = 1; outer <= count; outer++)
        {
            for (int inner = 0; inner < outer - 1; inner++)
            {
                Vector2 first = v2array[inner];
                Vector2 second = v2array[inner + 1];

                if (Comparer(first, second))
                {
                    temp = v2array[inner];
                    v2array[inner] = v2array[inner + 1];
                    v2array[inner + 1] = temp;
                }
            }
        }
    }

    /// <summary>
    /// Create a perpendicular offset point at a position located along a line segment.
    /// </summary>
    /// <param name="a">Input. PointD(x,y) of p1.</param>
    /// <param name="b">Input. PointD(x,y) of p2.</param>
    /// <param name="position">Distance between p1(0.0) and p2 (1.0) in a percentage.</param>
    /// <param name="offset">Distance from position at 90degrees to p1 and p2- non-percetange based.</param>
    /// <param name="c">Output of the calculated point along p1 and p2. might not be necessary for the ultimate output.</param>
    /// <param name="d">Output of the calculated offset point.</param>
    static internal void PerpOffset(Vector2 a, Vector2 b, float position, float offset, out Vector2 c, out Vector2 d)
    {
        //p3 is located at the x or y delta * position + p1x or p1y original.
        var p3 = new Vector2(((b.X - a.X) * position) + a.X, ((b.Y - a.Y) * position) + a.Y);

        //returns an angle in radians between p1 and p2 + 1.5708 (90degress).
        var angleRadians = MathF.Atan2(a.Y - b.Y, a.X - b.X) + 1.5708f;

        //locates p4 at the given angle and distance from p3.
        var p4 = new Vector2(p3.X + MathF.Cos(angleRadians) * offset, p3.Y + MathF.Sin(angleRadians) * offset);

        //send out the calculated points
        c = p3;
        d = p4;
    }
}
