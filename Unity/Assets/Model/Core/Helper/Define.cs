﻿using UnityEngine;

namespace Model
{
    public static class Define
    {
        /// <summary>
        /// 存放AB的父目录名字   会影响打包AB的时候与之对应的信息类文件的名字
        /// </summary>
        public static string ABsPathParent { get; } = "AssetBundles";

        #region 资源路径

        private static string hotfixResPath;
        private static string resPath;

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

        #endregion
    }
}