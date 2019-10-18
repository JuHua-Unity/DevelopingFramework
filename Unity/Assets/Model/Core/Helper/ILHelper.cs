using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;

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

            #endregion

            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();

            CLRBindings.Initialize(appdomain);

            #region appdomain.RegisterCrossBindingAdaptor



            #endregion

            #region appdomain.RegisterValueTypeBinder



            #endregion

            //LitJson注册
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
        }
    }
}