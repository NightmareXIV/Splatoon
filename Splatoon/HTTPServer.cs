using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splatoon
{
    class HTTPServer : IDisposable
    {
        const int port = 47774;
        HttpListener listener;
        Splatoon p;
        public HTTPServer(Splatoon p)
        {
            this.p = p;
            listener = new HttpListener()
            {
                Prefixes = { "http://127.0.0.1:" + port + "/" }
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
                        var elementsName = request.QueryString.Get("name");
                        var directElements = request.QueryString.Get("elements");
                        var destroyElements = request.QueryString.Get("destroy");
                        var destroyAt = request.QueryString.Get("destroyAt");
                        var enableElements = request.QueryString.Get("enable");
                        var disableElements = request.QueryString.Get("disable");
                        if(directElements != null && elementsName == null)
                        {
                            status.Add("Warning: elements param is present but name is not, direct elements will not be processed!");
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
                                p.tickScheduler.Enqueue(delegate 
                                {
                                    for (var i = p.dynamicElements.Count - 1; i >= 0; i++)
                                    {
                                        var de = p.dynamicElements[i];
                                        if(de.Name == s)
                                        {
                                            p.dynamicElements.RemoveAt(i);
                                        }
                                    }
                                });
                            }
                        }

                        if (elementsName != null && directElements != null)
                        {
                            var dynElem = new DynamicElement()
                            {
                                DestroyTime = 0,
                                Name = elementsName,
                            };

                            var Layouts = new List<Layout>();
                            var Elements = new List<Element>();
                            if (long.TryParse(destroyAt, out var dAt))
                            {
                                dynElem.DestroyTime = (DestroyAt)dAt;
                            }
                            var encodedElements = directElements.Split(',');
                            foreach (var e in encodedElements)
                            {
                                var decoded = Static.Decompress(e);
                                if (decoded.StartsWith("~"))
                                {
                                    Layouts.Add(JsonConvert.DeserializeObject<Layout>(decoded.Substring(1)));
                                }
                                else
                                {
                                    Elements.Add(JsonConvert.DeserializeObject<Element>(decoded));
                                }
                            }
                            dynElem.Elements = Elements.ToArray();
                            dynElem.Layouts = Layouts.ToArray();
                            p.tickScheduler.Enqueue(delegate 
                            {
                                p.dynamicElements.Add(dynElem);
                            });
                        }
                        HttpListenerResponse response = context.Response;
                        response.AppendHeader("Access-Control-Allow-Origin", "*");
                        string responseString = "ok";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                    catch (Exception e)
                    {
                        //p._pi.Framework.Gui.Chat.Print("Error: " + e + "\n" + e.StackTrace);
                    }
                }
            }).Start();
        }

        public void Dispose()
        {
            listener.Abort();
            listener = null;
        }
    }
}
