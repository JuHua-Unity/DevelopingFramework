using System.Collections.Generic;
using FairyGUI;
#if DEFINE_LOCALRES && UNITY_EDITOR
using UnityEditor;

#else
using UnityEngine;

#endif

namespace Hotfix
{
    internal class FUIPackagesComponent : Component, IAwakeSystem
    {
        public static FUIPackagesComponent Instance { get; private set; }

        private readonly Dictionary<string, PackageInfoComponent> packages = new Dictionary<string, PackageInfoComponent>();

        public void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkgName">fairygui编辑器命名的包名</param>
        /// <param name="descName">unity命名的AssetBundle名字 描述文件即UI 不需要.unity3d</param>
        /// <param name="resName">unity命名的AssetBundle名字 资源文件 不需要.unity3d</param>
        public void AddPackage(string pkgName, string descName, string resName = null)
        {
            if (this.packages.TryGetValue(pkgName, out var packageInfo))
            {
                packageInfo.RefCount++;
                return;
            }

            descName = AssetBundleNameHelper.CollectBundleName(descName);
            resName = string.IsNullOrEmpty(resName) ? resName : AssetBundleNameHelper.CollectBundleName(resName);
            AddOnePackage(descName, resName, pkgName);
        }

        public void RemovePackage(string pkgName)
        {
            if (!this.packages.TryGetValue(pkgName, out var packageInfo))
            {
                Log.Error($"包{pkgName}不存在却要卸载！");
                return;
            }

            packageInfo.RefCount--;
            if (packageInfo.RefCount > 0)
            {
                return;
            }

            RemoveMultiComponent(packageInfo);
        }

        private void AddOnePackage(string descName, string resName, string pkgName)
        {
#if DEFINE_LOCALRES && UNITY_EDITOR
            var realPath = AssetDatabase.GetAssetPathsFromAssetBundle(descName);
            foreach (var s in realPath)
            {
                if (s.EndsWith("_fui.bytes"))
                {
                    var packageInfo = AddMultiComponent<PackageInfoComponent, string, string>(s.Replace("_fui.bytes", string.Empty), pkgName);
                    this.packages[pkgName] = packageInfo;
                }
            }

#else
            var desc = ResourcesComponent.Instance.GetAssetBundle(descName);
            if (string.IsNullOrEmpty(resName))
            {
                var packageInfo = AddMultiComponent<PackageInfoComponent, AssetBundle, string>(desc, pkgName);
                this.packages[pkgName] = packageInfo;
            }
            else
            {
                var res = ResourcesComponent.Instance.GetAssetBundle(resName);
                var packageInfo = AddMultiComponent<PackageInfoComponent, AssetBundle, AssetBundle, string>(desc, res, pkgName);
                this.packages[pkgName] = packageInfo;
            }

#endif
        }

#if DEFINE_LOCALRES && UNITY_EDITOR
        private class PackageInfoComponent : Component, IDestroySystem, IAwakeSystem<string, string>
        {
            public string Name { get; set; }
            public int RefCount { get; set; }
            public string DescFilePath { get; set; }

            public void Awake(string a, string b)
            {
                this.Name = b;
                this.RefCount = 1;
                this.DescFilePath = a;
                UIPackage.AddPackage(a);
            }

            public void Destroy()
            {
                this.RefCount = 0;
                UIPackage.RemovePackage(this.Name);
                this.DescFilePath = null;
            }
        }

#else
        private class PackageInfoComponent : Component, IAwakeSystem<AssetBundle, AssetBundle, string>, IDestroySystem, IAwakeSystem<AssetBundle, string>
        {
            public string Name { get; set; }
            public int RefCount { get; set; }
            public AssetBundle DescAssetBundle { get; private set; }
            public AssetBundle ResAssetBundle { get; private set; }

            public void Awake(AssetBundle a, AssetBundle b, string c)
            {
                this.Name = c;
                this.RefCount = 1;
                this.DescAssetBundle = a;
                this.ResAssetBundle = b;
                UIPackage.AddPackage(a, b);
            }

            public void Awake(AssetBundle a, string b)
            {
                this.Name = b;
                this.RefCount = 1;
                this.DescAssetBundle = a;
                this.ResAssetBundle = null;
                UIPackage.AddPackage(a);
            }

            public void Destroy()
            {
                this.RefCount = 0;
                UIPackage.RemovePackage(this.Name);
                this.DescAssetBundle?.Unload(true);
                this.ResAssetBundle?.Unload(true);
            }
        }

#endif
    }
}