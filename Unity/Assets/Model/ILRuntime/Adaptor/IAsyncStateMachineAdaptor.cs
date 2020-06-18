using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Runtime.CompilerServices;

namespace Model
{
    [Adaptor]
    public class IAsyncStateMachineAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(IAsyncStateMachine);
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

        public class Adaptor : CrossBindingAdaptorType, IAsyncStateMachine
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

            private IMethod moveNext;

            public void MoveNext()
            {
                if (moveNext == null)
                {
                    moveNext = ILInstance.Type.GetMethod("MoveNext", 0);
                }

                appdomain.Invoke(moveNext, ILInstance, null);
            }

            private IMethod setStateMachine;
            private readonly object[] setStateMachineParam = new object[1];

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                if (setStateMachine == null)
                {
                    setStateMachine = ILInstance.Type.GetMethod("SetStateMachine", 1);
                }

                setStateMachineParam[0] = stateMachine;
                appdomain.Invoke(moveNext, ILInstance, setStateMachineParam);
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