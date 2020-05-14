using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public static class Define
    {
        #region AssetBundle相关

        /// <summary>
        /// 存放AB的父目录名字   会影响打包AB的时候与之对应的信息类文件的名字
        /// </summary>
        public const string ABsPathParent = "AssetBundles";

        /// <summary>
        /// AB变体名
        /// </summary>
        public const string ABVariant = "unity3d";

        /// <summary>
        /// 打包AB之后的版本文件名字
        /// </summary>
        public const string ResVersionJson = "AssetBundlesVersion.json";

        #region AssetBundle名字转换缓存池

        private static readonly Dictionary<string, string> bundleNames = new Dictionary<string, string>();

        public static string ToBundleName(this string bundleName)
        {
            if (bundleNames.TryGetValue(bundleName, out var n))
            {
                return n;
            }

            n = $"{bundleName}.{ABVariant}".ToLower();
            bundleNames.Add(bundleName, n);
            return n;
        }

        #endregion

        #endregion

        #region Code

        public const string HotfixDll = "Hotfix.dll";
        public const string HotfixPdb = "Hotfix.pdb";

        #endregion

        #region 资源路径

        private static string hotfixResPath;
        private static string resPath;
        private static string resPathForWeb;

        /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath => hotfixResPath ?? (hotfixResPath = Application.isMobilePlatform
                                                     ? $"{Application.persistentDataPath}/{Application.productName}/{ABsPathParent}/"
                                                     : AppResPath);

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                if (resPath != null)
                {
                    return resPath;
                }

                resPath = $"{Application.streamingAssetsPath}/{ABsPathParent}/";
#if UNITY_EDITOR
                resPath = $"{Application.dataPath}/{ABsPathParent}/";
#endif

                return resPath;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径(www/webrequest专用)
        /// </summary>
        public static string AppResPathForWeb
        {
            get
            {
                if (resPathForWeb == null)
                {
                    resPathForWeb = AppResPath;
#if !UNITY_EDITOR
#if UNITY_IOS || UNITY_STANDALONE_OSX
                    resPathForWeb = $"file://{AppResPath}";
#endif
#endif
                }

                return resPathForWeb;
            }
        }

        #endregion
    }
}