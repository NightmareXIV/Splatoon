namespace Splatoon
{
    partial class CGui
    {
        void DisplayLog()
        {
            ImGui.Checkbox("Autoscroll##log", ref autoscrollLog);
            ImGui.SameLine();
            if (ImGui.Button("Copy all"))
            {
                var s = new StringBuilder();
                for (int i = 0; i < p.LogStorage.Length; i++)
                {
                    if (p.LogStorage[i] != null)
                    {
                        s.AppendLine(p.LogStorage[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                ImGui.SetClipboardText(s.ToString());
            }

            ImGui.Checkbox("Copy in Dalamud.log##log", ref p.Config.dumplog);
            ImGui.SameLine();
            ImGui.Checkbox("Verbose##log", ref p.Config.verboselog);
            ImGui.BeginChild("##splatoondbg2");
            for (var i = 0; i < p.LogStorage.Length; i++)
            {
                if (p.LogStorage[i] != null) ImGui.TextWrapped(p.LogStorage[i]);
            }
            if (autoscrollLog) ImGui.SetScrollHereY();
            ImGui.EndChild();
        }
    }
}
