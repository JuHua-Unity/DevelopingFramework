using System.Collections.Generic;

namespace Hotfix
{
    internal class PlayerPrefsComponent : Component, IDestroySystem
    {
        #region 实例

        private static PlayerPrefsComponent inst;

        public static PlayerPrefsComponent Instance => inst ?? (inst = Game.ComponentRoot.AddComponent<PlayerPrefsComponent>());

        public static bool Active => inst != null;

        #endregion

        //private Dictionary<string,>

        #region Destroy

        public void Destroy()
        {
            //优先设置为null 防止后面调用的时候继续可用
            inst = null;
        }

        #endregion

        private class Data
        {
            public string Key { get; set; }
            public float Value1 { get; set; }
            public int Value2 { get; set; }
            public bool Value3 { get; set; }
            public string Value4 { get; set; }
        }
    }
}