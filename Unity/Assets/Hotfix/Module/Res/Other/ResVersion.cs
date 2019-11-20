using System.Collections.Generic;

namespace Hotfix
{
    public sealed class ResVersion
    {
        /// <summary>
        /// 组 以及组里面的资源
        /// 资源可以属于多个组
        /// </summary>
        public Dictionary<string, List<ResVersionInfo>> Res;

        public class ResVersionInfo
        {
            public string File { get; set; }
            public string MD5 { get; set; }
            public long Size { get; set; }
        }
    }
}