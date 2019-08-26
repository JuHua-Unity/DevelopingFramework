using System;

namespace Model
{
    internal interface IStartSystem
    {
        Type Type();
        void Run(object o);
    }

    internal abstract class StartSystem<T> : IStartSystem
    {
        public void Run(object o)
        {
            Start((T)o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public abstract void Start(T self);
    }
}
