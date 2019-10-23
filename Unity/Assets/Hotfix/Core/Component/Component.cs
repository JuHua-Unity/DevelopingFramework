using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 组件基类
    /// 万物皆组件
    /// </summary>
    internal abstract partial class Component : Object
    {
#if UNITY_EDITOR
        public Component()
        {
            ObjName = GetType().Name;
            if (GameObject == null)
            {
                GameObject = new GameObject(ObjName);
                GameObject.transform.SetParent(Global.transform, false);
                GameObject.AddComponent<Model.ComponentView>().Component = this;
            }
        }
#endif

        public static GameObject Global { get; } = GameObject.Find("/GameRoot");
#if UNITY_EDITOR
        public GameObject GameObject { get; private set; } = null;
#endif
        private Component parent;

        public Component Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
#if UNITY_EDITOR
                if (parent == null)
                {
                    GameObject.transform.SetParent(Global.transform, false);
                }
                else
                {
                    GameObject.transform.SetParent(parent.GameObject.transform, false);
                }
#endif
            }
        }

        public T GetParent<T>() where T : Component
        {
            return Parent as T;
        }
    }
}