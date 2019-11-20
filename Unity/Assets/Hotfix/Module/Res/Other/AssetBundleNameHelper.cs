using System.Collections.Generic;

namespace Hotfix
{
    internal static class AssetBundleNameHelper
    {
        private static readonly Dictionary<string, string> bundleNames = new Dictionary<string, string>();

        public static string CollectBundleName(string bundleName)
        {
            if (!bundleNames.TryGetValue(bundleName, out string n))
            {
                n = $"{bundleName}.unity3d".ToLower();
                bundleNames.Add(bundleName, n);
            }

            return n;
        }
    }
}