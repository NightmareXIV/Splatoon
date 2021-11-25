using Dalamud.Game.Command;
using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Internal.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatoon
{
    class Commands : IDisposable
    {
        Splatoon p;
        internal Commands(Splatoon p)
        {
            this.p = p;
            Svc.Commands.AddHandler("/splatoon", new CommandInfo(delegate (string command, string arguments)
            {
                if (arguments == "")
                {
                    p.ConfigGui.Open = true;
                }
                else if (arguments.StartsWith("enable "))
                {
                    try
                    {
                        var name = arguments.Substring(arguments.IndexOf("enable ") + 7);
                        SwitchState(name, true);
                    }
                    catch (Exception e)
                    {
                        p.Log(e.Message);
                    }
                }
                else if (arguments.StartsWith("disable "))
                {
                    try
                    {
                        var name = arguments.Substring(arguments.IndexOf("disable ") + 8);
                        SwitchState(name, false);
                    }
                    catch (Exception e)
                    {
                        p.Log(e.Message);
                    }
                }
                else if (arguments.StartsWith("settarget "))
                {
                    try
                    {
                        if (Svc.Targets.Target == null)
                        {
                            Svc.PluginInterface.UiBuilder.AddNotification("Target not selected", "Splatoon", NotificationType.Error);
                        }
                        else 
                        {
                            var name = arguments.Substring(arguments.IndexOf("settarget ") + 10).Split('~');
                            p.Config.Layouts[name[0]].Elements[name[1]].refActorName = Svc.Targets.Target.Name.ToString();
                            Svc.PluginInterface.UiBuilder.AddNotification("Successfully set target", "Splatoon", NotificationType.Success);
                        }
                    }
                    catch (Exception e)
                    {
                        p.Log(e.Message);
                    }
                }
                else if(arguments.StartsWith("floodchat "))
                {
                    int a = 2;
                    Safe(delegate
                    {
                        for(var i = 0;i<uint.Parse(arguments.Replace("floodchat ", "")); i++)
                        {
                            Svc.Chat.Print(new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 30).Select(s => s[new Random().Next(30)]).ToArray()));
                        }
                    });
                }
            })
            {
                HelpMessage = "open Splatoon configuration menu \n" +
                "/splatoon disable <PresetName> → disable specified preset \n" +
                "/splatoon enable <PresetName> → enable specified preset"
            });

            Svc.Commands.AddHandler("/sf", new CommandInfo(delegate (string command, string arguments)
            {
                if (arguments == "")
                {
                    if (p.SFind != null)
                    {
                        Svc.PluginInterface.UiBuilder.AddNotification("[Splatoon] Search stopped", "Splatoon", NotificationType.Info);
                        p.SFind = null;
                    }
                    else
                    {
                        Svc.PluginInterface.UiBuilder.AddNotification("[Splatoon] Please specify target name", "Splatoon", NotificationType.Error);
                    }
                }
                else
                {
                    p.SFind = arguments.Trim();
                    Svc.PluginInterface.UiBuilder.AddNotification("[Splatoon] Searching for: " + p.SFind, "Splatoon", NotificationType.Success);
                }
            })
            {
                HelpMessage = "highlight objects containing specified phrase"
            });
        }

        internal void SwitchState(string name, bool enable, bool web = false)
        {
            try
            {
                if (name.Contains("~"))
                {
                    var aname = name.Split('~');
                    if (web && p.Config.Layouts[aname[0]].DisableDisabling) return;
                    p.Config.Layouts[aname[0]].Elements[aname[1]].Enabled = enable;
                }
                else
                {
                    if (web && p.Config.Layouts[name].DisableDisabling) return;
                    p.Config.Layouts[name].Enabled = enable;
                }
            }
            catch(Exception e)
            {
                p.Log(e.Message, true);
                p.Log(e.StackTrace);
            }
        }

        public void Dispose()
        {
            Svc.Commands.RemoveHandler("/splatoon");
            Svc.Commands.RemoveHandler("/sf");
        }
    }
}
