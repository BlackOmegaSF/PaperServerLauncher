using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaperServerLauncher
{
    static class Utils
    {
        public enum Constants
        {
            UNIT_MODE_MB = 0,
            UNIT_MODE_GB = 1
        }

        public static class Maths
        {
            public static UInt64 roundToPowerOf2(double number)
            {
                UInt64 power = 1;
                while (power < number)
                    power*= 2;
                return power / 2;
            }

        }

        public class AikarFlag
        {
            private bool Enabled {  get; set; }
            public int? NumValue { get; set; }
            public string Name { get; set; }
            public string StrValue { get; set; }
            public string RawName { get; set; }

            public AikarFlag(string name, bool enabled, int? numValue, string strValue, string rawName)
            {
                Name = name;
                Enabled = enabled;
                StrValue = strValue;
                NumValue = numValue;
                RawName = rawName;

            }

        }

        public class AikarFlagData
        {
            BindingSource source = new BindingSource();
            List<AikarFlag> flagsList = new List<AikarFlag>()
            {
                new AikarFlag("UseG1GC", true, null, null, "-XX:+UseG1GC"),
                new AikarFlag("ParallelRefProcEnabled", true, null, null, "-XX:+ParallelRefProcEnabled"),
                new AikarFlag("MaxGCPauseMillis", true, 200, null, "-XX:MaxGCPauseMillis"),
                new AikarFlag("UnlockExperimentalVMOptions", true, null, null, "-XX:+UnlockExperimentalVMOptions"),
                new AikarFlag("DisableExplicitGC", true, null, null, "-XX:+DisableExplicitGC"),
                new AikarFlag("AlwaysPreTouch", true, null, null, "-XX:+AlwaysPreTouch"),
                new AikarFlag("G1NewSizePercent", true, 30, null, "-XX:G1NewSizePercent"),
                new AikarFlag("G1MaxNewSizePercent", true, 40, null, "-XX:G1MaxNewSizePercent"),
                new AikarFlag("G1HeapRegionSize", true, 8, null, "-XX:G1HeapRegionSize"),
                new AikarFlag("G1ReservePercent", true, 20, null, "-XX:G1ReservePercent"),
                new AikarFlag("G1HeapWastePercent", true, 5, null, "-XX:G1HeapWastePercent"),
                new AikarFlag("G1MixedGCCountTarget", true, 4, null, "-XX:G1MixedGCCountTarget"),
                new AikarFlag("InitiatingHeapOccupancyPercent", true, 15, null, "-XX:InitiatingHeapOccupancyPercent"),
                new AikarFlag("G1MixedGCLiveThresholdPercent", true, 90, null, "-XX:G1MixedGCLiveThresholdPercent"),
                new AikarFlag("G1RSetUpdatingPauseTimePercent", true, 5, null, "-XX:G1RSetUpdatingPauseTimePercent"),
                new AikarFlag("SurvivorRatio", true, 32, null, "-XX:SurvivorRatio"),
                new AikarFlag("PerfDisableSharedMem", true, null, null, "-XX:+PerfDisableSharedMem"),
                new AikarFlag("MaxTenuringThreshold", true, 1, null, "-XX:MaxTenuringThreshold"),
                new AikarFlag("UsingAikarsFlags", true, null, "https://mcflags.emc.gs", "-Dusing.aikars.flags"),
                new AikarFlag("AikarsNewFlags", true, null, "true", "-Daikars.new.flags"),
            }
        }
        
    }
}
