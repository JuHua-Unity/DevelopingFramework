using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Reflection;

namespace Model
{
    public static class ILHelper
    {
        public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            #region appdomain.DelegateManager.RegisterDelegateConvertor



            #endregion

            #region appdomain.DelegateManager.RegisterFunctionDelegate



            #endregion

            #region appdomain.DelegateManager.RegisterMethodDelegate

            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Object>();
            appdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            #endregion

            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();

            CLRBindings.Initialize(appdomain);

            #region appdomain.RegisterCrossBindingAdaptor

            Assembly assembly = typeof(Init).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(AdaptorAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                object obj = Activator.CreateInstance(type);
                if (!(obj is CrossBindingAdaptor adaptor))
                {
                    continue;
                }

                appdomain.RegisterCrossBindingAdaptor(adaptor);
            }

            #endregion

            #region appdomain.RegisterValueTypeBinder



            #endregion

            //LitJson注册
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
        }
    }
}