//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace Model
{
    public partial class GameEntry
    {
        public static HotfixComponent Hotfix { get; private set; }

        private static void InitCustomComponents()
        {
            Hotfix = UnityGameFramework.Runtime.GameEntry.GetComponent<HotfixComponent>();
        }
    }
}