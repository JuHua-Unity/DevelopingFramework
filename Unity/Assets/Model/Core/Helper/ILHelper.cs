#if ILRuntime
namespace Model
{
    public static class ILHelper
    {
        public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            m_1.Clear();
            m_2.Clear();
            m_3.Clear();
            m_4.Clear();
            f_0.Clear();
            f_1.Clear();
            f_2.Clear();
            f_3.Clear();
            f_4.Clear();
#endif

            #region appdomain.DelegateManager.RegisterDelegateConvertor



            #endregion

            #region appdomain.DelegateManager.RegisterFunctionDelegate



            #endregion

            #region appdomain.DelegateManager.RegisterMethodDelegate

            RegisterMethodDelegate<System.Boolean>(appdomain);
            RegisterMethodDelegate<System.Int32, System.Object>(appdomain);
            RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance>(appdomain);


            #endregion

            RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>(appdomain);

            ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);

            #region appdomain.RegisterCrossBindingAdaptor

            System.Reflection.Assembly assembly = typeof(Init).Assembly;
            foreach (System.Type type in assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(AdaptorAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                object obj = System.Activator.CreateInstance(type);
                if (!(obj is ILRuntime.Runtime.Enviorment.CrossBindingAdaptor adaptor))
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

        public static void RegisterMethodDelegate<T1>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1));
            if (m_1.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterMethodDelegate<{a.ToString()}>");
            }
            else
            {
                m_1.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1>();
        }

        public static void RegisterMethodDelegate<T1, T2>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2));
            if (m_2.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterMethodDelegate<{a.ToString()}>");
            }
            else
            {
                m_2.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1, T2>();
        }

        public static void RegisterMethodDelegate<T1, T2, T3>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3));
            if (m_3.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterMethodDelegate<{a.ToString()}>");
            }
            else
            {
                m_3.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1, T2, T3>();
        }

        public static void RegisterMethodDelegate<T1, T2, T3, T4>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            if (m_4.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterMethodDelegate<{a.ToString()}>");
            }
            else
            {
                m_4.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1, T2, T3, T4>();
        }

        public static void RegisterFunctionDelegate<TResult>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(TResult));
            if (f_0.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterFunctionDelegate<{a.ToString()}>");
            }
            else
            {
                f_0.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<TResult>();
        }

        public static void RegisterFunctionDelegate<T1, TResult>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(TResult));
            if (f_1.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterFunctionDelegate<{a.ToString()}>");
            }
            else
            {
                f_1.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, TResult>();
        }

        public static void RegisterFunctionDelegate<T1, T2, TResult>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(TResult));
            if (f_2.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterFunctionDelegate<{a.ToString()}>");
            }
            else
            {
                f_2.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, T2, TResult>();
        }

        public static void RegisterFunctionDelegate<T1, T2, T3, TResult>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3), typeof(TResult));
            if (f_3.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterFunctionDelegate<{a.ToString()}>");
            }
            else
            {
                f_3.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, T2, T3, TResult>();
        }

        public static void RegisterFunctionDelegate<T1, T2, T3, T4, TResult>(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(TResult));
            if (f_4.Contains(a))
            {
                UnityEngine.Debug.LogError($"重复注册：RegisterFunctionDelegate<{a.ToString()}>");
            }
            else
            {
                f_4.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, T2, T3, T4, TResult>();
        }

#if UNITY_EDITOR
        private static readonly System.Collections.Generic.List<TClass> m_1 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> m_2 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> m_3 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> m_4 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> f_0 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> f_1 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> f_2 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> f_3 = new System.Collections.Generic.List<TClass>();
        private static readonly System.Collections.Generic.List<TClass> f_4 = new System.Collections.Generic.List<TClass>();

        //防止重复注册
        private class TClass
        {
            private readonly System.Collections.Generic.List<System.Type> list = new System.Collections.Generic.List<System.Type>();

            public TClass(params System.Type[] types)
            {
                list.Clear();
                if (types == null)
                {
                    return;
                }

                for (int i = 0; i < types.Length; i++)
                {
                    list.Add(types[i]);
                }
            }

            public System.Type[] Types { get { return list.ToArray(); } }

            public override bool Equals(object obj)
            {
                if (obj is TClass s)
                {
                    var ts1 = s.Types;
                    var ts2 = Types;
                    if (ts1.Length != ts2.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < ts1.Length; i++)
                    {
                        if (!ts1[i].Equals(ts2[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                string str = "";
                for (int i = 0; i < list.Count; i++)
                {
                    str += $"{list[i].FullName}, ";
                }

                return str.Trim().Trim(',').Trim();
            }
        }
#endif
    }
}
#endif