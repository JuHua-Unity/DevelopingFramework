using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 组件基类
    /// 万物皆组件
    /// </summary>
    internal abstract partial class Component : Object
    {
#if UNITY_EDITOR && !ILRuntime && ComponentView

        public Component()
        {
            ObjName = GetType().Name;
            if (GameObject == null)
            {
                GameObject = new GameObject(ObjName);
                GameObject.transform.SetParent(GameRoot.transform, false);
                GameObject.AddComponent<Model.ComponentView>().Component = this;
            }
        }

#endif

        public static GameObject GameRoot { get; set; } = null;

#if UNITY_EDITOR && !ILRuntime && ComponentView

        public GameObject GameObject { get; private set; } = null;

        public static GameObject ParentNullRoot { get; set; } = null;

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

#if UNITY_EDITOR && !ILRuntime && ComponentView

                if (parent == null)
                {
                    GameObject.transform.SetParent(ParentNullRoot.transform, false);
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