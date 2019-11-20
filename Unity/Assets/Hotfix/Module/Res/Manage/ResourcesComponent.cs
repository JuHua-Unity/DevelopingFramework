using Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Hotfix
{
    internal sealed class ResourcesComponent : Component, IDestroySystem, IAwakeSystem
    {
        public static ResourcesComponent Instance { get; private set; } = null;

#if DEFINE_LOCALRES && UNITY_EDITOR
#else
        private AssetBundleManifest assetBundleManifest;
#endif

        private readonly Dictionary<string, ABInfoComponent> res = new Dictionary<string, ABInfoComponent>();

        public UnityEngine.Object GetAsset(string bundleName, string prefab)
        {
            bundleName = AssetBundleNameHelper.CollectBundleName(bundleName);

            if (!res.TryGetValue(bundleName, out ABInfoComponent abInfo))
            {
                return null;
            }

            if (!abInfo.Objects.TryGetValue(prefab, out UnityEngine.Object obj))
            {
                return null;
            }

            return obj;
        }

        public AssetBundle GetAssetBundle(string bundleName)
        {
            bundleName = AssetBundleNameHelper.CollectBundleName(bundleName);

            if (!res.TryGetValue(bundleName, out ABInfoComponent abInfo))
            {
                return null;
            }

            return abInfo.AssetBundle;
        }

        public void Load(string bundleName)
        {
            string[] dependencies = GetSortedDependencies(AssetBundleNameHelper.CollectBundleName(bundleName));
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
            string[] dependencies = GetSortedDependencies(AssetBundleNameHelper.CollectBundleName(bundleName));
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }

                await LoadOneAsync(dependency);
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
            LoadOne(Define.ABsPathParent);
        }

        public async Task LoadMainABAsync()
        {
            await LoadOneAsync(Define.ABsPathParent);
        }

        public void Unload(string bundleName)
        {
            string[] dependencies = GetSortedDependencies(AssetBundleNameHelper.CollectBundleName(bundleName));
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }

                UnloadOne(dependency);
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
            UnloadOne(Define.ABsPathParent);
        }

        private void LoadOne(string bundleName)
        {
            if (res.TryGetValue(bundleName, out ABInfoComponent a))
            {
                a.RefCount++;
                return;
            }

            if (LoadOne_LocalRes(bundleName))
            {
                return;
            }

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

            ABInfoComponent abInfo = AddMultiComponent<ABInfoComponent, AssetBundle>(assetBundle);
            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
                foreach (UnityEngine.Object asset in assets)
                {
                    abInfo.Objects.Add(asset.name, asset);
                }
            }

            res.Add(bundleName, abInfo);
        }

        private async Task LoadOneAsync(string bundleName)
        {
            if (res.TryGetValue(bundleName, out ABInfoComponent a))
            {
                a.RefCount++;
                return;
            }

            if (LoadOne_LocalRes(bundleName))
            {
                return;
            }

            string p = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
            if (!File.Exists(p))
            {
                p = Path.Combine(PathHelper.AppResPath, bundleName);
            }

            AssetsBundleLoaderAsync assetsBundleLoader = AddMultiComponent<AssetsBundleLoaderAsync>();
            AssetBundle assetBundle = await assetsBundleLoader.LoadAsync(p);
            RemoveMultiComponent(assetsBundleLoader);
            if (assetBundle == null)
            {
                throw new System.Exception($"assets bundle not found: {bundleName}");
            }

            ABInfoComponent abInfo = AddMultiComponent<ABInfoComponent, AssetBundle>(assetBundle);
            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                AssetsLoaderAsync assetsLoader = AddMultiComponent<AssetsLoaderAsync>();
                UnityEngine.Object[] assets = await assetsLoader.LoadAllAssetsAsync(assetBundle);
                RemoveMultiComponent(assetsLoader);
                foreach (UnityEngine.Object asset in assets)
                {
                    res[bundleName].Objects.Add(asset.name, asset);
                }
            }
        }

        private bool LoadOne_LocalRes(string bundleName)
        {
#if DEFINE_LOCALRES && UNITY_EDITOR
            string[] realPath = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
            ABInfoComponent abInfo = AddMultiComponent<ABInfoComponent, AssetBundle>(null);
            foreach (string s in realPath)
            {
                string assetName = Path.GetFileNameWithoutExtension(s);
                UnityEngine.Object resource = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
                abInfo.Objects.Add(resource.name, resource);
            }

            res.Add(bundleName, abInfo);

            return true;
#else
            return false;
#endif
        }

        private void UnloadOne(string bundleName)
        {
            if (res.TryGetValue(bundleName, out ABInfoComponent a))
            {
                a.RefCount--;
                if (a.RefCount > 0)
                {
                    return;
                }

                RemoveMultiComponent(a);
            }
            else
            {
                Log.Error($"资源[{bundleName}]没有加载却要卸载！");
            }
        }

        #region interface

        public void Awake()
        {
            Instance = this;

#if DEFINE_LOCALRES && UNITY_EDITOR
#else
            LoadMainAB();
            assetBundleManifest = (AssetBundleManifest)GetAsset(Define.ABsPathParent, "AssetBundleManifest");
            UnloadMainAB();
#endif
        }

        public void Destroy()
        {
            res.Clear();
            dependenciesCache.Clear();
#if DEFINE_LOCALRES && UNITY_EDITOR
#else
            assetBundleManifest = null;
#endif
        }

        #endregion

        private class ABInfoComponent : Component, IAwakeSystem<AssetBundle>, IDestroySystem
        {
            public int RefCount { get; set; }
            public AssetBundle AssetBundle { get; private set; }

            public readonly Dictionary<string, UnityEngine.Object> Objects = new Dictionary<string, UnityEngine.Object>();

            public void Awake(AssetBundle b)
            {
                RefCount = 1;
                AssetBundle = b;
            }

            public void Destroy()
            {
                RefCount = 0;
                AssetBundle?.Unload(true);
                Objects.Clear();
            }
        }

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
            dependencies = UnityEditor.AssetDatabase.GetAssetBundleDependencies(bundleName, true);
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