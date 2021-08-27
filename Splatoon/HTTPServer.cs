using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Splatoon
{
    class HTTPServer : IDisposable
    {
        HttpListener listener;
        Splatoon p;
        public HTTPServer(Splatoon p)
        {
            this.p = p;
            listener = new HttpListener()
            {
                Prefixes = { "http://127.0.0.1:" + p.Config.port + "/" }
            };
            listener.Start();
            new Thread((ThreadStart)delegate
            {
                while (listener != null && listener.IsListening)
                {
                    try
                    {
                        List<string> status = new List<string>();
                        HttpListenerContext context = listener.GetContext();
                        HttpListenerRequest request = context.Request;
                        var elementsName = request.QueryString.Get("namespace");
                        var directElements = request.QueryString.Get("elements");
                        var destroyElements = request.QueryString.Get("destroy");
                        var destroyAt = request.QueryString.Get("destroyAt");
                        var enableElements = request.QueryString.Get("enable");
                        var disableElements = request.QueryString.Get("disable");
                        var rawElement = request.QueryString.Get("raw");
                        try
                        {
                            if (elementsName == null)
                            {
                                elementsName = "";
                            }
                            if (disableElements != null)
                            {
                                var names = disableElements.Split(',');
                                foreach (var n in names)
                                {
                                    p.tickScheduler.Enqueue(delegate { p.CommandManager.SwitchState(n, false); });
                                    status.Add("Disabling: " + n);
                                }
                            }

                            if (enableElements != null)
                            {
                                var names = enableElements.Split(',');
                                foreach (var n in names)
                                {
                                    p.tickScheduler.Enqueue(delegate { p.CommandManager.SwitchState(n, true); });
                                    status.Add("Enabling: " + n);
                                }
                            }

                            if (destroyElements != null)
                            {
                                foreach (var s in destroyElements.Split(','))
                                {
                                    status.Add("Requesting destruction: " + s);
                                    p.tickScheduler.Enqueue(delegate
                                    {
                                        for (var i = p.dynamicElements.Count - 1; i >= 0; i--)
                                        {
                                            var de = p.dynamicElements[i];
                                            if (de.Name == s)
                                            {
                                                p.dynamicElements.RemoveAt(i);
                                            }
                                        }
                                    });
                                }
                            }

                            if (directElements != null || rawElement != null)
                            {
                                var dynElem = new DynamicElement()
                                {
                                    DestroyTime = 0,
                                    Name = elementsName,
                                };

                                var Layouts = new List<Layout>();
                                var Elements = new List<Element>();
                                if (destroyAt != null)
                                {
                                    if (long.TryParse(destroyAt, out var dAt) && dAt > 0)
                                    {
                                        dynElem.DestroyTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + dAt;
                                    }
                                    else
                                    {
                                        dynElem.DestroyTime = (long)Enum.Parse(typeof(DestroyCondition), destroyAt, true);
                                    }
                                }
                                if (rawElement != null)
                                {
                                    status.Add("Raw payload found");
                                    //status.Add(rawElement);
                                    ProcessElement(rawElement, ref Layouts, ref Elements);
                                }
                                if(directElements != null)
                                {
                                    var encodedElements = directElements.Split(',');
                                    foreach (var e in encodedElements)
                                    {
                                        //status.Add(directElements);
                                        var decoded = Static.Decompress(e);
                                        ProcessElement(decoded, ref Layouts, ref Elements);
                                    }
                                }
                                dynElem.Elements = Elements.ToArray();
                                dynElem.Layouts = Layouts.ToArray();
                                p.tickScheduler.Enqueue(delegate
                                {
                                    p.dynamicElements.Add(dynElem);
                                });
                                status.Add($"Requesting dynamic element addition: {dynElem.Name} (Elements: {dynElem.Elements.Length}, " +
                                    $"Layouts: {dynElem.Layouts.Length}, destroyAt: {dynElem.DestroyTime})");
                            }
                        }
                        catch(Exception e)
                        {
                            status.Add("Error:");
                            status.Add(e.Message);
                            status.Add(e.StackTrace);
                        }
                        HttpListenerResponse response = context.Response;
                        response.AppendHeader("Access-Control-Allow-Origin", "*");
                        string responseString = string.Join("\n", status);
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                    catch (Exception e)
                    {
                        //p.pi.Framework.Gui.Chat.Print("Error: " + e + "\n" + e.StackTrace);
                    }
                }
            }).Start();
        }

        private void ProcessElement(string decoded, ref List<Layout> Layouts, ref List<Element> Elements)
        {
            if (decoded.StartsWith("~"))
            {
                //status.Add(decoded);
                var l = JsonConvert.DeserializeObject<Layout>(decoded.Substring(1));
                l.Enabled = true;
                foreach (var el in l.Elements.Values) el.Enabled = true;
                Layouts.Add(l);
            }
            else
            {
                var l = JsonConvert.DeserializeObject<Element>(decoded);
                l.Enabled = true;
                Elements.Add(l);
            }
        }

        public void Dispose()
        {
            listener.Abort();
            listener = null;
        }
    }
}
