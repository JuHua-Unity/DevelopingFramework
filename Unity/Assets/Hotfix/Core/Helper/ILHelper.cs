#if ILRuntime
namespace Hotfix
{
    internal static class ILHelper
    {
        /// <summary>
        /// 注册
        /// 不用重新打包
        /// 只支持下面四类
        /// </summary>
        public static void InitILRuntime()
        {
            var appdomain = Model.Game.Hotfix.AppDomain;

            #region appdomain.DelegateManager.RegisterDelegateConvertor

            #endregion

            #region appdomain.DelegateManager.RegisterFunctionDelegate

            #endregion

            #region appdomain.DelegateManager.RegisterMethodDelegate

            #endregion

            #region appdomain.RegisterValueTypeBinder

            #endregion
        }
    }
}
#endif