namespace Hotfix
{
    /// <inheritdoc />
    /// <summary>
    /// 组件基类
    /// 万物皆组件
    /// </summary>
    internal abstract partial class Component : Object
    {
        private Component parent;

        public Component Parent
        {
            get { return this.parent; }
            set
            {
                this.parent = value;

#if UNITY_EDITOR && !ILRuntime && ComponentView

                SetParent(this.parent?.GameObject);
#endif
            }
        }

        public T GetParent<T>() where T : Component
        {
            return this.Parent as T;
        }
    }
}