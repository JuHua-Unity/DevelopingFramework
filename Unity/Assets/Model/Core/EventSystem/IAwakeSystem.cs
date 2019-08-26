using System;

namespace Model
{
    internal interface IAwakeSystem
    {
        Type Type();
    }

    internal interface IAwake
    {
        void Run(object o);
    }

    internal interface IAwake<A>
    {
        void Run(object o, A a);
    }

    internal interface IAwake<A, B>
    {
        void Run(object o, A a, B b);
    }

    internal interface IAwake<A, B, C>
    {
        void Run(object o, A a, B b, C c);
    }

    internal abstract class AwakeSystem<T> : IAwakeSystem, IAwake
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o)
        {
            Awake((T)o);
        }

        public abstract void Awake(T self);
    }

    internal abstract class AwakeSystem<T, A> : IAwakeSystem, IAwake<A>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o, A a)
        {
            Awake((T)o, a);
        }

        public abstract void Awake(T self, A a);
    }

    internal abstract class AwakeSystem<T, A, B> : IAwakeSystem, IAwake<A, B>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o, A a, B b)
        {
            Awake((T)o, a, b);
        }

        public abstract void Awake(T self, A a, B b);
    }

    internal abstract class AwakeSystem<T, A, B, C> : IAwakeSystem, IAwake<A, B, C>
    {
        public Type Type()
        {
            return typeof(T);
        }

        public void Run(object o, A a, B b, C c)
        {
            Awake((T)o, a, b, c);
        }

        public abstract void Awake(T self, A a, B b, C c);
    }
}
