using System.Collections.Generic;

namespace Model
{
    public sealed class ResControl
    {
        public string PlatformName = string.Empty;
        public string LatestAppVersion = "0.0.1";
        public List<App_ResData> App_Res = new List<App_ResData>();
        public List<string> TestUsers = new List<string>();

        public class App_ResData
        {
            public string AppVersion { get; set; }
            public string ResVersion { get; set; }
            public string TestResVersion { get; set; }
        }
    }
}