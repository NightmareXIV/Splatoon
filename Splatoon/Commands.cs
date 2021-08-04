using Dalamud.Game.Command;
using Dalamud.Game.Internal.Gui.Toast;
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
            p.pi.CommandManager.AddHandler("/splatoon", new CommandInfo(delegate (string command, string arguments)
            {
                if (arguments == "")
                {
                    p.ConfigGui.Open = true;
                }
                else if (arguments == "d")
                {
                    p.DebugGui.Open = true;
                }
                else if (arguments.StartsWith("enable "))
                {
                    try
                    {
                        var name = arguments.Substring(arguments.IndexOf("enable ") + 7);
                        p.Config.Layouts[name].Enabled = true;
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
                        p.Config.Layouts[name].Enabled = false;
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
                        if (p.pi.ClientState?.Targets?.CurrentTarget == null)
                        {
                            p.pi.Framework.Gui.Toast.ShowError("Target not selected");
                        }
                        else 
                        {
                            var name = arguments.Substring(arguments.IndexOf("settarget ") + 10).Split('~');
                            p.Config.Layouts[name[0]].Elements[name[1]].refActorName = p.pi.ClientState.Targets.CurrentTarget.Name;
                            p.pi.Framework.Gui.Toast.ShowQuest("Successfully set target");
                        }
                    }
                    catch (Exception e)
                    {
                        p.Log(e.Message);
                    }
                }
            })
            {
                HelpMessage = "open Splatoon configuration menu \n" +
                "/splatoon disable <PresetName> → disable specified preset \n" +
                "/splatoon enable <PresetName> → enable specified preset"
            });

            p.pi.CommandManager.AddHandler("/sf", new CommandInfo(delegate (string command, string arguments)
            {
                if (arguments == "")
                {
                    if (p.SFind != null)
                    {
                        p.pi.Framework.Gui.Toast.ShowNormal("[Splatoon] Search stopped", new ToastOptions()
                        {
                            Position = ToastPosition.Top
                        });
                        p.SFind = null;
                    }
                    else
                    {
                        p.pi.Framework.Gui.Toast.ShowError("[Splatoon] Please specify target name");
                    }
                }
                else
                {
                    p.SFind = arguments.Trim();
                    p.pi.Framework.Gui.Toast.ShowQuest("[Splatoon] Searching for: " + p.SFind, new QuestToastOptions()
                    {
                        DisplayCheckmark = true,
                        PlaySound = true
                    });
                }
            })
            {
                HelpMessage = "highlight objects containing specified phrase"
            });
        }

        public void Dispose()
        {
            p.pi.CommandManager.RemoveHandler("/splatoon");
            p.pi.CommandManager.RemoveHandler("/sf");
        }
    }
}
