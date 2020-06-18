using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;

namespace Model
{
    //[Adaptor]
    public class ITestAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(ITest);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

        public class Adaptor : ITest, CrossBindingAdaptorType
        {
            private ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adaptor() { }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                ILInstance = instance;
            }

            public ILTypeInstance ILInstance { get; }

            #region 这里写适配的方法

            private IMethod m;

            public void M()
            {
                if (m == null)
                {
                    m = ILInstance.Type.GetMethod("M", 0);
                }

                appdomain.Invoke(m, ILInstance, null);
            }

            #endregion

            private bool toStringGot = false;
            private IMethod toStringMethod = null;

            public override string ToString()
            {
                if (!toStringGot)
                {
                    toStringMethod = appdomain.ObjectType.GetMethod("ToString", 0);
                    toStringMethod = ILInstance.Type.GetVirtualMethod(toStringMethod);
                    toStringGot = true;
                }

                if (toStringMethod == null || toStringMethod is ILMethod)
                {
                    return ILInstance.ToString();
                }

                return ILInstance.Type.FullName;
            }
        }
    }
}