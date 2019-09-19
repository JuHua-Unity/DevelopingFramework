using UnityEngine;

namespace Model
{
    internal static class PathHelper
    {
        /// <summary>
        /// 存放AB的父目录名字   会影响打包AB的时候与之对应的信息类文件的名字
        /// </summary>
        public static string ResPathParent { get; } = "AB";

        /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath
        {
            get
            {
                return Application.isMobilePlatform
                    ? $"{Application.persistentDataPath}/{Application.productName}/{ResPathParent}/"
                    : AppResPath;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                return $"{Application.streamingAssetsPath}/{ResPathParent}/";
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径(www/webrequest专用)
        /// </summary>
        public static string AppResPath4Web
        {
            get
            {
#if UNITY_IOS || UNITY_STANDALONE_OSX
                return $"file://{Application.streamingAssetsPath}/{ResPathParent}/";
#else
                return AppResPath;
#endif
            }
        }
    }
}
