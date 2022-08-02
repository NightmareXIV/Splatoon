using Dalamud.Game.Command;
using ECommons.GameFunctions;

namespace Splatoon;

class Commands : IDisposable
{
    Splatoon p;
    internal unsafe Commands(Splatoon p)
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
                        Notify.Error("Target not selected");
                    }
                    else 
                    {
                        var name = arguments.Substring(arguments.IndexOf("settarget ") + 10).Split('~');
                        var el = p.Config.LayoutsL.First(x => x.Name == name[0]).ElementsL.First(x => x.Name == name[1]);
                        el.refActorNameIntl.CurrentLangString = Svc.Targets.Target.Name.ToString();
                        el.refActorDataID = Svc.Targets.Target.DataId;
                        el.refActorObjectID = Svc.Targets.Target.ObjectId;
                        if (Svc.Targets.Target is Character c) el.refActorModelID = (uint)c.Struct()->ModelCharaId;
                        Notify.Success("Successfully set target");
                    }
                }
                catch (Exception e)
                {
                    p.Log(e.Message);
                }
            }
            else if(arguments.StartsWith("floodchat "))
            {
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
                    Notify.Info("Search stopped");
                    p.SFind = null;
                }
                else
                {
                    Notify.Error("Please specify target name");
                }
            }
            else
            {
                p.SFind = new()
                {
                    name = arguments.Trim(),
                    includeUntargetable = arguments.StartsWith("!!")
                };
                if (p.SFind.includeUntargetable)
                {
                    p.SFind.name = arguments[2..];
                }
                Notify.Success("Searching for: " + p.SFind.name + (p.SFind.includeUntargetable?" (+untargetable)":""));
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
                foreach (var x in P.Config.LayoutsL.Where(x => x.Name == aname[0]))
                {
                    if (web && x.DisableDisabling) continue;
                    foreach(var z in x.ElementsL.Where(z => z.Name == aname[1]))
                    {
                        z.Enabled = enable;
                    }
                }
            }
            else
            {
                foreach (var x in P.Config.LayoutsL.Where(x => x.Name == name))
                {
                    if (web && x.DisableDisabling) continue;
                    x.Enabled = enable;
                }
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
