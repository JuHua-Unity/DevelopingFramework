using ObjectDrawer;
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

        public GameObject GameObject { get; }
        public static GameObject ParentNullRoot { get; set; } = null;

        protected Component()
        {
            this.ObjName = GetType().Name;
            if (this.GameObject != null)
            {
                return;
            }

            this.GameObject = new GameObject(this.ObjName);
            this.GameObject.transform.SetParent(GameRoot.transform, false);
            this.GameObject.AddComponent<ObjectView>().Obj = this;
        }

#endif

        public static GameObject GameRoot { get; set; } = null;

        private Component parent;

        public Component Parent
        {
            get { return this.parent; }
            set
            {
                this.parent = value;

#if UNITY_EDITOR && !ILRuntime && ComponentView

                this.GameObject.transform.SetParent(this.parent == null ? ParentNullRoot.transform : this.parent.GameObject.transform, false);

#endif
            }
        }

        public T GetParent<T>() where T : Component
        {
            return this.Parent as T;
        }
    }
}