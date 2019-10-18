using Model;
using UnityEngine;

namespace Hotfix
{
    internal static class PathHelper
    {
        private static string hotfixResPath = null;
        private static string resPath = null;
        private static string resPathForWeb = null;

        /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath
        {
            get
            {
                if (hotfixResPath == null)
                {
                    hotfixResPath = Application.isMobilePlatform
                        ? $"{Application.persistentDataPath}/{Application.productName}/{Define.ABsPathParent}/"
                        : AppResPath;
                }

                return hotfixResPath;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                if (resPath == null)
                {
                    resPath = $"{Application.streamingAssetsPath}/{Define.ABsPathParent}/";
                }

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
#if UNITY_IOS || UNITY_STANDALONE_OSX
                    resPathForWeb = $"file://{Application.streamingAssetsPath}/{Define.ResPathParent}/";
#else
                    resPathForWeb = AppResPath;
#endif
                }

                return resPathForWeb;
            }
        }
    }
}
