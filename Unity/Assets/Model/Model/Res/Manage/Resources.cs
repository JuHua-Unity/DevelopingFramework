using System.Collections.Generic;
using System.IO;
using System.Linq;
#if DEFINE_LOCALRES && UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Model
{
    public sealed class Resources
    {
        #region 更新下载部分

        private readonly ResUpdate resUpdate = new ResUpdate();
        private const string initGroup = "Init";

        public async Task InitDownload(LaunchOptions options)
        {
            resUpdate.SetNeedUpdate(!string.IsNullOrEmpty(options.GateServerURL) && !string.IsNullOrEmpty(options.ResServerURL));
            resUpdate.SetUrl(options.ResServerURL);
            if (!await resUpdate.UpdateRes(initGroup))
            {
                Log.Error("初始资源更新失败，本次将使用本地资源！");
            }
        }

        #endregion

        #region 资源管理部分

        private AssetBundleManifest assetBundleManifest;
        private readonly Dictionary<string, ABInfo> res = new Dictionary<string, ABInfo>();

        public async Task InitResources()
        {
            string main = PathHelper.ResPathParent;
            if (assetBundleManifest != null)
            {
                if (res.ContainsKey(main))
                {
                    res[main].Dispose();
                    Game.Pool.Recycle(res[main]);
                    res.Remove(main);
                }

                dependenciesCache.Clear();
            }

            await LoadMainABAsync();
            assetBundleManifest = GetAsset(main, "AssetBundleManifest") as AssetBundleManifest;
            UnloadMainAB();
        }

        public Object GetAsset(string bundleName, string prefab)
        {
            if (!res.TryGetValue(bundleName, out ABInfo abInfo))
            {
                return null;
            }

            if (!abInfo.Objects.TryGetValue(prefab, out Object obj))
            {
                return null;
            }

            return obj;
        }

        public void Load(string bundleName)
        {
            string[] dependencies = GetSortedDependencies(CollectBundleName(bundleName));
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }

                LoadOne(dependency);
            }
        }

        public void Load(params string[] bundleNames)
        {
            for (int i = 0; i < bundleNames.Length; i++)
            {
                Load(bundleNames[i]);
            }
        }

        public void Load(IEnumerable<string> bundleNames)
        {
            foreach (string item in bundleNames)
            {
                Load(item);
            }
        }

        public async Task LoadAsync(string bundleName)
        {
            string[] dependencies = GetSortedDependencies(CollectBundleName(bundleName));
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }

                await LoadOneAsync(CollectBundleName(dependency));
            }
        }

        public async Task LoadAsync(params string[] bundleNames)
        {
            for (int i = 0; i < bundleNames.Length; i++)
            {
                await LoadAsync(bundleNames[i]);
            }
        }

        public async Task LoadAsync(IEnumerable<string> bundleNames)
        {
            foreach (string item in bundleNames)
            {
                await LoadAsync(item);
            }
        }

        public void LoadMainAB()
        {
            LoadOne(PathHelper.ResPathParent);
        }

        public async Task LoadMainABAsync()
        {
            await LoadOneAsync(PathHelper.ResPathParent);
        }

        public void Unload(string bundleName)
        {
            string[] dependencies = GetSortedDependencies(CollectBundleName(bundleName));
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }

                if (res.TryGetValue(dependency, out ABInfo a))
                {
                    a.Dispose();
                    Game.Pool.Recycle(a);
                }
            }
        }

        public void Unload(params string[] bundleNames)
        {
            for (int i = 0; i < bundleNames.Length; i++)
            {
                Unload(bundleNames[i]);
            }
        }

        public void Unload(IEnumerable<string> bundleNames)
        {
            foreach (string item in bundleNames)
            {
                Unload(item);
            }
        }

        public void UnloadMainAB()
        {
            if (res.TryGetValue(PathHelper.ResPathParent, out ABInfo a))
            {
                a.Dispose();
                Game.Pool.Recycle(a);
            }
        }

        private void LoadOne(string bundleName)
        {
            if (res.TryGetValue(bundleName, out ABInfo a))
            {
                a.RefCount++;
                return;
            }

#if DEFINE_LOCALRES && UNITY_EDITOR
            string[] realPath = null;
            realPath = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
            foreach (string s in realPath)
            {
                string assetName = Path.GetFileNameWithoutExtension(s);
                Object resource = AssetDatabase.LoadAssetAtPath<Object>(s);
                if (!res.TryGetValue(bundleName, out ABInfo abInfo))
                {
                    res.Add(bundleName, Game.Pool.Fetch<ABInfo>());
                }

                res[bundleName].RefCount = 1;
                res[bundleName].AssetBundle = null;
                res[bundleName].Objects.Add(assetName, resource);
            }

            return;
#endif
            string p = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
            if (!File.Exists(p))
            {
                p = Path.Combine(PathHelper.AppResPath, bundleName);
            }
            AssetBundle assetBundle = AssetBundle.LoadFromFile(p);

            if (assetBundle == null)
            {
                throw new System.Exception($"assets bundle not found: {bundleName}");
            }

            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                Object[] assets = assetBundle.LoadAllAssets();

                res.Add(bundleName, Game.Pool.Fetch<ABInfo>());
                res[bundleName].RefCount = 1;
                res[bundleName].AssetBundle = assetBundle;

                foreach (Object asset in assets)
                {
                    res[bundleName].Objects.Add(asset.name, asset);
                }
            }
        }

        private async Task LoadOneAsync(string bundleName)
        {
            if (res.TryGetValue(bundleName, out ABInfo a))
            {
                a.RefCount++;
                return;
            }

#if DEFINE_LOCALRES && UNITY_EDITOR
            string[] realPath = null;
            realPath = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
            foreach (string s in realPath)
            {
                string assetName = Path.GetFileNameWithoutExtension(s);
                Object resource = AssetDatabase.LoadAssetAtPath<Object>(s);
                if (!res.TryGetValue(bundleName, out ABInfo abInfo))
                {
                    res.Add(bundleName, Game.Pool.Fetch<ABInfo>());
                }

                res[bundleName].RefCount = 1;
                res[bundleName].AssetBundle = null;
                res[bundleName].Objects.Add(assetName, resource);
            }

            return;
#endif
            string p = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
            if (!File.Exists(p))
            {
                p = Path.Combine(PathHelper.AppResPath, bundleName);
            }

            AssetsBundleLoaderAsync assetsBundleLoader = Game.Pool.Fetch<AssetsBundleLoaderAsync>();
            AssetBundle assetBundle = await assetsBundleLoader.LoadAsync(p);
            assetsBundleLoader.Dispose();
            Game.Pool.Recycle(assetsBundleLoader);

            if (assetBundle == null)
            {
                throw new System.Exception($"assets bundle not found: {bundleName}");
            }

            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                AssetsLoaderAsync assetsLoader = Game.Pool.Fetch<AssetsLoaderAsync>();
                Object[] assets = await assetsLoader.LoadAllAssetsAsync(assetBundle);
                assetsLoader.Dispose();
                Game.Pool.Recycle(assetsLoader);

                res.Add(bundleName, Game.Pool.Fetch<ABInfo>());
                res[bundleName].RefCount = 1;
                res[bundleName].AssetBundle = assetBundle;

                foreach (Object asset in assets)
                {
                    res[bundleName].Objects.Add(asset.name, asset);
                }
            }
        }

        #region BundleNameCache

        private readonly Dictionary<string, string> bundleNames = new Dictionary<string, string>();

        private string CollectBundleName(string bundleName)
        {
            if (!bundleNames.TryGetValue(bundleName, out string n))
            {
                n = $"{bundleName}.unity3d".ToLower();
            }

            return n;
        }

        #endregion

        private class ABInfo
        {
            public int RefCount { get; set; }
            public AssetBundle AssetBundle { get; set; }
            public readonly Dictionary<string, Object> Objects = new Dictionary<string, Object>();

            public void Dispose()
            {
                RefCount = 0;
                AssetBundle?.Unload(true);
                Objects.Clear();
            }
        }

        #endregion

        #region Dependencies

        private Dictionary<string, string[]> dependenciesCache = new Dictionary<string, string[]>();

        private string[] GetDependencies(string bundleName)
        {
            string[] dependencies = new string[0];
            if (dependenciesCache.TryGetValue(bundleName, out dependencies))
            {
                return dependencies;
            }

#if DEFINE_LOCALRES && UNITY_EDITOR
            dependencies = AssetDatabase.GetAssetBundleDependencies(bundleName, true);
#else
            dependencies = assetBundleManifest.GetAllDependencies(bundleName);
#endif
            dependenciesCache.Add(bundleName, dependencies);

            return dependencies;
        }

        private string[] GetSortedDependencies(string bundleName)
        {
            Dictionary<string, int> info = new Dictionary<string, int>();
            List<string> parents = new List<string>();
            CollectDependencies(parents, bundleName, info);
            string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            return ss;
        }

        private void CollectDependencies(List<string> parents, string bundleName, Dictionary<string, int> info)
        {
            parents.Add(bundleName);
            string[] deps = GetDependencies(bundleName);
            foreach (string parent in parents)
            {
                if (!info.ContainsKey(parent))
                {
                    info[parent] = 0;
                }
                info[parent] += deps.Length;
            }

            foreach (string dep in deps)
            {
                if (parents.Contains(dep))
                {
                    throw new System.Exception($"包有循环依赖，请重新标记: {bundleName} {dep}");
                }
                CollectDependencies(parents, dep, info);
            }

            parents.RemoveAt(parents.Count - 1);
        }

        #endregion
    }
}