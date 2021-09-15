using Dalamud.Game;
using Dalamud.Game.ClientState.Conditions;
using Lumina.Excel.GeneratedSheets;

namespace Splatoon;
unsafe class Splatoon : IDalamudPlugin
{
    public string Name => "Splatoon";
    internal Gui DrawingGui;
    internal CGui ConfigGui;
    internal Commands CommandManager;
    internal Memory MemoryManager;
    internal ChlogGui ChangelogGui;
    internal Configuration Config;
    internal Dictionary<ushort, TerritoryType> Zones;
    internal string[] LogStorage = new string[100];
    internal long CombatStarted = 0;
    internal HashSet<DisplayObject> displayObjects = new HashSet<DisplayObject>();
    internal double CamAngleX;
    internal Dictionary<int, string> Jobs = new Dictionary<int, string>();
    internal HashSet<(float x, float y, float z, float r)> draw = new HashSet<(float x, float y, float z, float r)>();
    internal float CamAngleY;
    internal float CamZoom;
    internal bool S2WActive = false;
    internal bool prevMouseState = false;
    internal string SFind = null;
    internal int CurrentLineSegments;
    internal ConcurrentQueue<System.Action> tickScheduler;
    internal List<DynamicElement> dynamicElements;
    internal HTTPServer HttpServer;
    internal bool prevCombatState = false;
    internal bool isPvpZone = false;

    public string AssemblyLocation { get => assemblyLocation; set => assemblyLocation = value; }
    private string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

    public void Dispose()
    {
        Config.Save();
        DrawingGui.Dispose();
        ConfigGui.Dispose();
        CommandManager.Dispose();
        Svc.ClientState.TerritoryChanged -= TerritoryChangedEvent;
        Svc.Framework.Update -= Tick;
        Svc.ClientState.Login -= OnLogin;
        SetupShutdownHttp(false);
        Svc.Chat.Print("Disposing");
    }

    public Splatoon(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Svc>();
        Svc.Chat.Print("Loaded");
        CommandManager = new Commands(this);
        Zones = Svc.Data.GetExcelSheet<TerritoryType>().ToDictionary(row => (ushort)row.RowId, row => row);
        Jobs = Svc.Data.GetExcelSheet<ClassJob>().ToDictionary(row => (int)row.RowId, row => row.Name.ToString());
        Svc.ClientState.TerritoryChanged += TerritoryChangedEvent;
        MemoryManager = new Memory(this);
        Config = Svc.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Config.Initialize(this);
        Svc.Framework.Update += Tick;
        DrawingGui = new Gui(this);
        ConfigGui = new CGui(this);
        if(ChlogGui.ChlogVersion > Config.ChlogReadVer)
        {
            ChangelogGui = new ChlogGui(this);
        }
        tickScheduler = new ConcurrentQueue<System.Action>();
        dynamicElements = new List<DynamicElement>();
        SetupShutdownHttp(Config.UseHttpServer);
        UpdatePvpZone(Svc.ClientState.TerritoryType);
        Svc.ClientState.Login += OnLogin;
    }

    internal void SetupShutdownHttp(bool enable)
    {
        if (enable)
        {
            if(HttpServer == null)
            {
                try
                {
                    HttpServer = new HTTPServer(this);
                }
                catch(Exception e)
                {
                    Log("Critical error occurred while starting HTTP server.", true);
                    Log(e.Message, true);
                    Log(e.StackTrace);
                    HttpServer = null;
                }
            }
        }
        else
        {
            if (HttpServer != null)
            {
                HttpServer.Dispose();
                HttpServer = null;
            }
        }
    }

    private void TerritoryChangedEvent(object sender, ushort e)
    {
        UpdatePvpZone(e);
        if (SFind != null)
        {
            SFind = null;
            Svc.Toasts.ShowQuest("[Splatoon] Search stopped");
        }
        for (var i = dynamicElements.Count - 1; i >= 0; i--)
        {
            var de = dynamicElements[i];
            foreach (var dt in de.DestroyTime)
            {
                if (dt == (long)DestroyCondition.TERRITORY_CHANGE)
                {
                    dynamicElements.RemoveAt(i);
                }
            }
        }
    }

    void OnLogin(object sender, EventArgs e)
    {
        UpdatePvpZone(Svc.ClientState.TerritoryType);
    }

    void UpdatePvpZone(uint terr)
    {
        try
        {
            isPvpZone = Svc.Data.GetExcelSheet<TerritoryType>().GetRow(terr).IsPvpZone;
        }
        catch (Exception)
        {
            isPvpZone = false;
        }
    }

    [HandleProcessCorruptedStateExceptions]
    public void Tick(Framework framework)
    {
        try
        {
            if(tickScheduler.TryDequeue(out var action))
            {
                action.Invoke();
            }
            displayObjects.Clear();
            if (Svc.ClientState.LocalPlayer == null) return;
            var pl = Svc.ClientState.LocalPlayer;
            if (Svc.ClientState.LocalPlayer.Address == IntPtr.Zero)
            {
                Log("Pointer to LocalPlayer.Address is zero");
                return;
            }
            CamAngleX = *MemoryManager.CameraAddressX + Math.PI;
            if (CamAngleX > Math.PI) CamAngleX -= 2 * Math.PI;
            CamAngleY = *MemoryManager.CameraAddressY;
            CamZoom = *MemoryManager.CameraZoom;
            /*Range conversion https://stackoverflow.com/questions/5731863/mapping-a-numeric-range-onto-another
            slope = (output_end - output_start) / (input_end - input_start)
            output = output_start + slope * (input - input_start) */
            CurrentLineSegments = (int)((3f + -0.108108f * (CamZoom - 1.5f)) * Config.lineSegments);

            if (Svc.Condition[ConditionFlag.InCombat])
            {
                if (CombatStarted == 0)
                {
                    CombatStarted = DateTimeOffset.Now.ToUnixTimeSeconds();
                }
            }
            else
            {
                if (CombatStarted != 0)
                {
                    CombatStarted = 0;
                }
            }

            //if (CamAngleY > Config.maxcamY) return;

            if(SFind != null)
            {
                var col = DateTimeOffset.Now.ToUnixTimeMilliseconds() % 1000 < 500 ? Colors.Red : Colors.Yellow;
                var findEl = new Element(1)
                {
                    thicc = 3f,
                    radius = 0f,
                    refActorName = SFind,
                    overlayText = "Search: " + SFind,
                    overlayVOffset = 1.7f,
                    overlayTextColor = col,
                    color = col,
                    includeHitbox = true,
                    onlyTargetable = true
                };
                ProcessElement(findEl);
            }

            foreach (var i in Config.Layouts.Values)
            {
                if (!IsLayoutVisible(i)) continue;
                foreach (var e in i.Elements.Values.ToArray())
                {
                    ProcessElement(e);
                }
            }

            for(var i = dynamicElements.Count-1; i>=0; i--)
            {
                var de = dynamicElements[i];

                foreach (var dt in de.DestroyTime)
                {
                    if (dt == (long)DestroyCondition.COMBAT_EXIT)
                    {
                        if (!Svc.Condition[ConditionFlag.InCombat] && prevCombatState)
                        {
                            dynamicElements.RemoveAt(i);
                            continue;
                        }
                    }
                    else if (dt > 0)
                    {
                        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > dt)
                        {
                            dynamicElements.RemoveAt(i);
                            continue;
                        }
                    }
                }
                foreach(var l in de.Layouts)
                {
                    if (!IsLayoutVisible(l)) continue;
                    foreach (var e in l.Elements.Values.ToArray())
                    {
                        ProcessElement(e);
                    }
                }
                foreach(var e in de.Elements)
                {
                    ProcessElement(e);
                }
            }
            prevCombatState = Svc.Condition[ConditionFlag.InCombat];
        }
        catch(Exception e)
        {
            Log("Caught exception: "+e.Message);
            Log(e.StackTrace);
        }
    }

    internal void ProcessElement(Element e)
    {
        if (!e.Enabled) return;
        draw.Clear();
        if (e.screen2world != 0)
        {
            var lmbdown = Bitmask.IsBitSet(Native.GetKeyState(0x01), 15);
            S2WActive = true;
            //1: editing absolute point 
            //2: editing main point
            //3: editing secondary point
            var mousePos = ImGui.GetIO().MousePos;
            if (Svc.GameGui.ScreenToWorld(new Vector2(mousePos.X, mousePos.Y), out var worldPos, Config.maxdistance * 5))
            {
                if (e.screen2world == 1 || e.screen2world == 2)
                {
                    e.refX = worldPos.X;
                    e.refY = worldPos.Z;
                    e.refZ = worldPos.Y;
                }
                else if (e.screen2world == 3)
                {
                    e.offX = worldPos.X;
                    e.offY = worldPos.Z;
                    e.offZ = worldPos.Y;
                }
            }
            if (!lmbdown && prevMouseState)
            {
                e.screen2world = 0;
                S2WActive = false;
            }
            prevMouseState = lmbdown;
        }
        if (e.screen2world != 0 && DateTimeOffset.Now.ToUnixTimeMilliseconds() % 500 < 250)
        {
            var x = e.screen2world == 3 ? e.offX : e.refX;
            var y = e.screen2world == 3 ? e.offY : e.refY;
            var z = e.screen2world == 3 ? e.offZ : e.refZ;
            displayObjects.Add(new DisplayObjectLine(x + 2f, y + 2f, z, x - 2f, y - 2f, z, 2f, Colors.Red));
            displayObjects.Add(new DisplayObjectLine(x - 2f, y + 2f, z, x + 2f, y - 2f, z, 2f, Colors.Red));
        }
        float radius = e.radius;
        if (e.type == 0)
        {
            draw.Add((e.refX, e.refY, e.refZ, radius));
        }
        else if (e.type == 1)
        {
            if (e.includeOwnHitbox) radius += Svc.ClientState.LocalPlayer.HitboxRadius;
            if (e.refActorType == 1)
            {
                draw.Add((GetPlayerPositionXZY().X, GetPlayerPositionXZY().Y,
                    GetPlayerPositionXZY().Z, radius));
            }
            else if (e.refActorType == 2 && Svc.Targets.Target != null
                && Svc.Targets.Target is BattleNpc)
            {
                if (e.includeHitbox) radius += Svc.Targets.Target.HitboxRadius;
                draw.Add((Svc.Targets.Target.GetPositionXZY().X, Svc.Targets.Target.GetPositionXZY().Y,
                    Svc.Targets.Target.GetPositionXZY().Z, radius));
            }
            else if (e.refActorType == 0 && e.refActorName.Length > 0)
            {
                foreach (var a in Svc.Objects)
                {
                    if ((e.refActorName == "*" || a.Name.ToString().ContainsIgnoreCase(e.refActorName))
                            && (!e.onlyTargetable || GetIsTargetable(a)))
                    {
                        var aradius = radius;
                        if (e.includeHitbox) aradius += a.HitboxRadius;
                        draw.Add((a.GetPositionXZY().X, a.GetPositionXZY().Y, a.GetPositionXZY().Z, aradius));
                    }
                }
            }

        }
        else if (e.type == 2)
        {
            if (ShouldDraw(e.offX, GetPlayerPositionXZY().X, e.offY, GetPlayerPositionXZY().Y)
                || ShouldDraw(e.refX, GetPlayerPositionXZY().X, e.refY, GetPlayerPositionXZY().Y))
                displayObjects.Add(new DisplayObjectLine(e.refX, e.refY, e.refZ, e.offX, e.offY, e.offZ, e.thicc, e.color));
        }
        if (draw.Count == 0) return;
        foreach (var (x, y, z, r) in draw)
        {
            if (!ShouldDraw(x + e.offX, GetPlayerPositionXZY().X, y + e.offY, GetPlayerPositionXZY().Y)) continue;
            if (e.thicc > 0)
            {
                if (r > 0)
                {
                    displayObjects.Add(new DisplayObjectCircle(x + e.offX, y + e.offY, z + e.offZ, r, e.thicc, e.color));
                }
                else
                {
                    displayObjects.Add(new DisplayObjectDot(x + e.offX, y + e.offY, z + e.offZ, e.thicc, e.color));
                }
            }
            if (e.overlayText.Length > 0)
            {
                displayObjects.Add(new DisplayObjectText(x + e.offX, y + e.offY, z + e.offZ + e.overlayVOffset, e.overlayText, e.overlayBGColor, e.overlayTextColor));
            }
        }
    }

    internal bool IsLayoutVisible(Layout i)
    {
        if (!i.Enabled) return false;
        if (i.ZoneLockH.Count > 0 && !i.ZoneLockH.Contains(Svc.ClientState.TerritoryType)) return false;
        if (i.JobLock != 0 && !Bitmask.IsBitSet(i.JobLock, (int)Svc.ClientState.LocalPlayer.ClassJob.Id)) return false;
        if ((i.DCond == 1 || i.DCond == 3) && !Svc.Condition[ConditionFlag.InCombat]) return false;
        if ((i.DCond == 2 || i.DCond == 3) && !Svc.Condition[ConditionFlag.BoundByDuty]) return false;
        if (i.DCond == 4 && !(Svc.Condition[ConditionFlag.InCombat]
            || Svc.Condition[ConditionFlag.BoundByDuty])) return false;
        if (i.Visibility == 1)
        {
            if (!Svc.Condition[ConditionFlag.InCombat]) return false;
            var tic = DateTimeOffset.Now.ToUnixTimeSeconds() - CombatStarted;
            if (tic < i.BattleTimeBegin || tic > i.BattleTimeEnd) return false;
        }
        return true;
    }

    public bool ShouldDraw(float x1, float x2, float y1, float y2)
    {
        return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) < Config.maxdistance * Config.maxdistance;
    }

    public void Log(string s, bool tochat = false)
    {
        if (tochat)
        {
            Svc.Chat.Print("[Splatoon]" + s);
        }
        if (Config.dumplog)
        {
            try { PluginLog.Log(s); } catch (Exception) { }
        }
        var line = DateTimeOffset.Now.ToString() + ": " + s;
        for (var i = 0; i < LogStorage.Length; i++)
        {
            if (LogStorage[i] == null)
            {
                LogStorage[i] = line;
                return;
            }
        }
        for (var i = 1; i < LogStorage.Length; i++)
        {
            LogStorage[i - 1] = LogStorage[i];
        }
        LogStorage[LogStorage.Length - 1] = line;
    }

    internal bool GetIsTargetable(GameObject a)
    {
        if(a is Character)
        {
            return MemoryManager.GetIsTargetable_Character(a.Address) != 0;
        }
        else
        {
            return MemoryManager.GetIsTargetable_GameObject(a.Address) != 0;
        }
    }

    public void HandleChat()
    {

    }
}
