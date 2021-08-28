using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return a.ToLower(CultureInfo.InvariantCulture).Contains(b.ToLower(CultureInfo.InvariantCulture));
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
    }
}
