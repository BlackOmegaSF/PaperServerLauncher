using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public int NumValue { get; set; }
            public string Name { get; set; }
            public string StrValue { get; set; }

            public AikarFlag(string name, bool enabled, int numValue, string strValue)
            {
                Name = name;
                Enabled = enabled;
                StrValue = strValue;
                NumValue = numValue;
            }

        }

        public class AikarFlagStates
        {
            public int maxGCPauseMillis = 200;
            public int g1NewSizePercent = 30;
            public int g1MaxNewSizePercent = 40;
            public int g1HeapRegionSize = 8;
            public int g1ReservePercent = 20;
            public int g1HeapWastePercent = 5;
            public int g1MixedGCCountTarget = 4;
            public int initiatingHeapOccupancyPercent = 15;
            public int g1MixedGCLiveThresholdPercent = 90;
            public int g1RSetUpdatingPauseTimePercent = 5;
            public int survivorRatio = 32;
            public int maxTenuringThreshold = 1;
            public string usingAikarsFlags = "https://mcflags.emc.gs";
            public bool aikarsNewFlags = true;
        }
        
    }
}
