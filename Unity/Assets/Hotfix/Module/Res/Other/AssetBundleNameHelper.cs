using System.Collections.Generic;
using Model;

namespace Hotfix
{
    internal static class AssetBundleNameHelper
    {
        private static readonly Dictionary<string, string> bundleNames = new Dictionary<string, string>();

        public static string CollectBundleName(string bundleName)
        {
            if (!bundleNames.TryGetValue(bundleName, out var n))
            {
                n = $"{bundleName}.{Define.ABVariant}".ToLower();
                bundleNames.Add(bundleName, n);
            }

            return n;
        }
    }
}