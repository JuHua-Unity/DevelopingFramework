using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Async;
using Model;
using UnityEngine;
#if DEFINE_LOCALRES && UNITY_EDITOR
using UnityEditor;

#endif

namespace Hotfix
{
    internal sealed class ResourcesComponent : Component, IDestroySystem, IAwakeSystem
    {
        public static ResourcesComponent Instance { get; private set; }

#if DEFINE_LOCALRES && UNITY_EDITOR
#else
        private AssetBundleManifest assetBundleManifest;
#endif

        private readonly Dictionary<string, ABInfoComponent> res = new Dictionary<string, ABInfoComponent>();

        public UnityEngine.Object GetAsset(string bundleName, string prefab)
        {
            bundleName = AssetBundleNameHelper.CollectBundleName(bundleName);
            return !this.res.TryGetValue(bundleName, out var abInfo) ? null : abInfo.Get(prefab);
        }

        public AssetBundle GetAssetBundle(string bundleName)
        {
            bundleName = AssetBundleNameHelper.CollectBundleName(bundleName);

            if (!this.res.TryGetValue(bundleName, out var abInfo))
            {
                return null;
            }

            return abInfo.AssetBundle;
        }

        public void Load(string bundleName)
        {
            var dependencies = GetSortedDependencies(AssetBundleNameHelper.CollectBundleName(bundleName));
            foreach (var dependency in dependencies)
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
            for (var i = 0; i < bundleNames.Length; i++)
            {
                Load(bundleNames[i]);
            }
        }

        public void Load(IEnumerable<string> bundleNames)
        {
            foreach (var item in bundleNames)
            {
                Load(item);
            }
        }

        public async Task LoadAsync(string bundleName)
        {
            var dependencies = GetSortedDependencies(AssetBundleNameHelper.CollectBundleName(bundleName));
            foreach (var dependency in dependencies)
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
            for (var i = 0; i < bundleNames.Length; i++)
            {
                await LoadAsync(bundleNames[i]);
            }
        }

        public async Task LoadAsync(IEnumerable<string> bundleNames)
        {
            foreach (var item in bundleNames)
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
            var dependencies = GetSortedDependencies(AssetBundleNameHelper.CollectBundleName(bundleName));
            foreach (var dependency in dependencies)
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
            for (var i = 0; i < bundleNames.Length; i++)
            {
                Unload(bundleNames[i]);
            }
        }

        public void Unload(IEnumerable<string> bundleNames)
        {
            foreach (var item in bundleNames)
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
            if (this.res.TryGetValue(bundleName, out var a))
            {
                a.RefCount++;
                return;
            }

            if (LoadOne_LocalRes(bundleName))
            {
                return;
            }

            var p = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
            if (!File.Exists(p))
            {
                p = Path.Combine(PathHelper.AppResPath, bundleName);
            }

            var assetBundle = AssetBundle.LoadFromFile(p);

            if (assetBundle == null)
            {
                throw new Exception($"assets bundle not found: {bundleName}");
            }

            var abInfo = AddMultiComponent<ABInfoComponent, AssetBundle>(assetBundle);
            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                var assets = assetBundle.LoadAllAssets();
                foreach (var asset in assets)
                {
                    abInfo.Add(asset.name, asset);
                }
            }

            this.res.Add(bundleName, abInfo);
        }

        private async Task LoadOneAsync(string bundleName)
        {
            if (this.res.TryGetValue(bundleName, out var a))
            {
                a.RefCount++;
                return;
            }

            if (LoadOne_LocalRes(bundleName))
            {
                return;
            }

            var p = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
            if (!File.Exists(p))
            {
                p = Path.Combine(PathHelper.AppResPath, bundleName);
            }

            var assetsBundleLoader = AddMultiComponent<AssetsBundleLoaderAsync>();
            var assetBundle = await assetsBundleLoader.LoadAsync(p);
            RemoveMultiComponent(assetsBundleLoader);
            if (assetBundle == null)
            {
                throw new Exception($"assets bundle not found: {bundleName}");
            }

            AddMultiComponent<ABInfoComponent, AssetBundle>(assetBundle);
            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                var assetsLoader = AddMultiComponent<AssetsLoaderAsync>();
                var assets = await assetsLoader.LoadAllAssetsAsync(assetBundle);
                RemoveMultiComponent(assetsLoader);
                foreach (var asset in assets)
                {
                    this.res[bundleName].Add(asset.name, asset);
                }
            }
        }

        private bool LoadOne_LocalRes(string bundleName)
        {
#if DEFINE_LOCALRES && UNITY_EDITOR
            var realPath = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
            var abInfo = AddMultiComponent<ABInfoComponent, AssetBundle>(null);
            foreach (var s in realPath)
            {
                var resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
                abInfo.Add(resource.name, resource);
            }

            this.res.Add(bundleName, abInfo);

            return true;
#else
            return false;
#endif
        }

        private void UnloadOne(string bundleName)
        {
            if (this.res.TryGetValue(bundleName, out var a))
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
                Error($"资源[{bundleName}]没有加载却要卸载！");
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
            this.res.Clear();
            this.dependenciesCache.Clear();
#if DEFINE_LOCALRES && UNITY_EDITOR
#else
            assetBundleManifest = null;
#endif
        }

        #endregion

        private class ABInfoComponent : Component, IAwakeSystem<AssetBundle>, IDestroySystem
        {
            private readonly Dictionary<string, UnityEngine.Object> Objects = new Dictionary<string, UnityEngine.Object>();

            public int RefCount { get; set; }
            public AssetBundle AssetBundle { get; private set; }


            public void Awake(AssetBundle b)
            {
                this.RefCount = 1;
                this.AssetBundle = b;
            }

            public void Destroy()
            {
                this.RefCount = 0;
                this.AssetBundle?.Unload(true);
                this.Objects.Clear();
            }

            public void Add(string key, UnityEngine.Object value)
            {
                this.Objects.Add(key, value);
            }

            public UnityEngine.Object Get(string key)
            {
                return this.Objects.ContainsKey(key) ? this.Objects[key] : null;
            }
        }

        #region Dependencies

        private readonly Dictionary<string, string[]> dependenciesCache = new Dictionary<string, string[]>();

        private string[] GetDependencies(string bundleName)
        {
            if (this.dependenciesCache.TryGetValue(bundleName, out var dependencies))
            {
                return dependencies;
            }

#if DEFINE_LOCALRES && UNITY_EDITOR
            dependencies = AssetDatabase.GetAssetBundleDependencies(bundleName, true);
#else
            dependencies = assetBundleManifest.GetAllDependencies(bundleName);
#endif
            this.dependenciesCache.Add(bundleName, dependencies);

            return dependencies;
        }

        private IEnumerable<string> GetSortedDependencies(string bundleName)
        {
            var info = new Dictionary<string, int>();
            var parents = new List<string>();
            CollectDependencies(parents, bundleName, info);
            var ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            return ss;
        }

        private void CollectDependencies(IList<string> parents, string bundleName, Dictionary<string, int> info)
        {
            parents.Add(bundleName);
            var dependencies = GetDependencies(bundleName);
            foreach (var parent in parents)
            {
                if (!info.ContainsKey(parent))
                {
                    info[parent] = 0;
                }

                info[parent] += dependencies.Length;
            }

            foreach (var dep in dependencies)
            {
                if (parents.Contains(dep))
                {
                    throw new Exception($"包有循环依赖，请重新标记: {bundleName} {dep}");
                }

                CollectDependencies(parents, dep, info);
            }

            parents.RemoveAt(parents.Count - 1);
        }

        #endregion
    }
}