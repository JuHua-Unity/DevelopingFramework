using System;

namespace Model
{
    internal interface ILateUpdateSystem
    {
        Type Type();
        void Run(object o);
    }

    internal abstract class LateUpdateSystem<T> : ILateUpdateSystem
    {
        public void Run(object o)
        {
            LateUpdate((T)o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public abstract void LateUpdate(T self);
    }
}
