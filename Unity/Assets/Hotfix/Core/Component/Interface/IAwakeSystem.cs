namespace Hotfix
{
    internal interface IAwakeSystem
    {
        void Awake();
    }

    internal interface IAwakeSystem<A>
    {
        void Awake(A a);
    }

    internal interface IAwakeSystem<A, B>
    {
        void Awake(A a, B b);
    }

    internal interface IAwakeSystem<A, B, C>
    {
        void Awake(A a, B b, C c);
    }
}