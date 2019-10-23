using UnityEditor;

namespace Editors
{
    /// <summary>
    /// WindowsPC Android IOS 三类平台
    /// </summary>
    internal class PlatformDefine : Editor
    {
#if UNITY_STANDALONE
        public static readonly BuildTargetGroup TargetGroup = BuildTargetGroup.Standalone;
        public static readonly BuildTarget Target = BuildTarget.StandaloneWindows64;
#elif UNITY_IOS
        public static readonly BuildTargetGroup TargetGroup = BuildTargetGroup.iOS;
        public static readonly BuildTarget Target = BuildTarget.iOS;
#elif UNITY_ANDROID
        public static readonly BuildTargetGroup TargetGroup = BuildTargetGroup.Android;
        public static readonly BuildTarget Target = BuildTarget.Android;
#endif
    }
}