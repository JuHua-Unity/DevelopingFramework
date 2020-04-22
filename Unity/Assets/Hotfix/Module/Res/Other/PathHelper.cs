﻿using Model;

namespace Hotfix
{
    internal static class PathHelper
    {
        private static string resPathForWeb;

        /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath => Define.AppHotfixResPath;

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath => Define.AppResPath;

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