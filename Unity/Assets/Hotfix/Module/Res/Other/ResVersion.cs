namespace Hotfix
{
    public sealed class ResVersion
    {
        public ResVersionInfo[] Res { get; set; }

        public class ResVersionInfo
        {
            public string File { get; set; }
            public string MD5 { get; set; }
            public long Size { get; set; }
        }
    }
}