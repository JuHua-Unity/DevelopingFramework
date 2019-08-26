using System;

namespace Model
{
    internal interface IUpdateSystem
    {
        Type Type();
        void Run(object o);
    }

    internal abstract class UpdateSystem<T> : IUpdateSystem
    {
        public void Run(object o)
        {
            Update((T)o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public abstract void Update(T self);
    }
}
