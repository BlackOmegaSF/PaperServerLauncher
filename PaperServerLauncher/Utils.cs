using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaperServerLauncher
{
    static class Utils
    {
        public static class Constants
        {
            public const int UNIT_MODE_MB = 0;
            public const int UNIT_MODE_GB = 1;
            public const int MIN_RAM_GB = 2;
            public const int MIN_RAM_MB = MIN_RAM_GB * 1024;
            public const string UPDATER_INFO_FILE_NAME = "BlackOmegaUpdaterInfo.json";
            public const string GITHUB_BASE_RELEASES_URL = "https://api.github.com/repos/";
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

        public static class Formatters
        {
            public static int getMinRam(int unitMode)
            {
                switch (unitMode)
                {
                    case Constants.UNIT_MODE_MB:
                        return Constants.MIN_RAM_MB;

                    case Constants.UNIT_MODE_GB:
                        return Constants.MIN_RAM_GB;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(unitMode));
                }
            }

            public static string getMinRamString(int unitMode)
            {
                switch (unitMode)
                {
                    case Constants.UNIT_MODE_MB:
                        return Constants.MIN_RAM_MB.ToString() + "MB";

                    case Constants.UNIT_MODE_GB:
                        return Constants.MIN_RAM_GB.ToString() + "GB";
                    default:
                        throw new ArgumentOutOfRangeException("unitMode");
                }
            }

            public static string getJVMRamString(int unitMode, int ram)
            {
                switch (unitMode)
                {
                    case Constants.UNIT_MODE_MB:
                        return "-Xmx" + ram.ToString() + "M -Xms" + ram.ToString() + "M";

                    case Constants.UNIT_MODE_GB:
                        return "-Xmx" + ram.ToString() + "G -Xms" + ram.ToString() + "G";

                    default:
                        throw new ArgumentOutOfRangeException("unitMode");
                }
            }
        }

        public class RepoInfo
        {
            public string name;
            public string releaseTag;
            public string downloadUrl;

            public RepoInfo(string name, string releaseTag, string downloadUrl)
            {
                this.name = name;
                this.releaseTag = Regex.Replace(releaseTag, "[^0-9.]", "");
                this.downloadUrl = downloadUrl;
            }
        }

        public static class NetworkUtils
        {
            public static RepoInfo GetLatestRelease(string owner, string repo)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Constants.GITHUB_BASE_RELEASES_URL + owner + "/" + repo + "/releases/latest");
                request.Method = "GET";
                request.Accept = "application/vnd.github.v3+json";
                request.UserAgent = "BlackOmegaSF";

                WebResponse response = request.GetResponse();
                //Check response status
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        JObject jsonResponse = JObject.Parse(reader.ReadToEnd());
                        response.Close();
                        return new RepoInfo(repo, (string)jsonResponse["tag_name"], (string)jsonResponse["assets"][0]["browser_download_url"]);
                    }
                } 
                else
                {
                    throw new HttpListenerException((int)((HttpWebResponse)response).StatusCode, ((HttpWebResponse)response).StatusDescription);
                }

            }
        }

        public class UpdateInfoItem
        {
            public string id;
            public string version;
            public string owner;
            public string repo;
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

        public static class AikarFlagData
        {
            public static List<AikarFlag> flagsList = new List<AikarFlag>()
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
            };

            public static string getAllFlagsString()
            {
                StringBuilder sb = new StringBuilder();
                foreach(AikarFlag flag in flagsList)
                {
                    sb.Append(flag.RawName);
                    sb.Append(" ");
                }
                return sb.ToString();
            }
        }
        
    }
}
