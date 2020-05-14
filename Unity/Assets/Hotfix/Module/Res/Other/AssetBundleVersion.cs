namespace Hotfix
{
    public sealed class AssetBundleVersion //开放给Editor使用
    {
        public Item[] Res { get; set; }

        public class Item
        {
            public string File { get; set; }
            public string MD5 { get; set; }
            public long Size { get; set; }
        }
    }
}