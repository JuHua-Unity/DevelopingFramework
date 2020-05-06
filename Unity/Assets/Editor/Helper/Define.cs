using UnityEditor;

namespace Editors
{
    internal class Define : Editor
    {
        public static readonly string BuildPath = "../Release/";
        public static readonly string EditorConfigsPath = "EditorConfigs/";

        // WindowsPC Android IOS 三类平台
#if UNITY_STANDALONE
        public static readonly BuildTargetGroup TargetGroup = BuildTargetGroup.Standalone;
        public static readonly BuildTarget Target = BuildTarget.StandaloneWindows64;
        public static readonly string TargetName = "Standalone";
#elif UNITY_IOS
        public static readonly BuildTargetGroup TargetGroup = BuildTargetGroup.iOS;
        public static readonly BuildTarget Target = BuildTarget.iOS;
        public static readonly string TargetName = "IOS";
#elif UNITY_ANDROID
        public static readonly BuildTargetGroup TargetGroup = BuildTargetGroup.Android;
        public static readonly BuildTarget Target = BuildTarget.Android;
        public static readonly string TargetName = "Android";
#endif
    }
}