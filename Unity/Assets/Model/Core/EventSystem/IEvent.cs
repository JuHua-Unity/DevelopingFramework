using System;

namespace Model
{
    internal interface IEvent
    {
        void Handle();
        void Handle(object a);
        void Handle(object a, object b);
        void Handle(object a, object b, object c);
    }

    internal abstract class AEvent : IEvent
    {
        public void Handle()
        {
            Run();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public abstract void Run();
    }

    internal abstract class AEvent<A> : IEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            Run((A)a);
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a);
    }

    internal abstract class AEvent<A, B> : IEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            Run((A)a, (B)b);
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a, B b);
    }

    internal abstract class AEvent<A, B, C> : IEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            Run((A)a, (B)b, (C)c);
        }

        public abstract void Run(A a, B b, C c);
    }
}