using System;
using System.Collections.Generic;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using LitJson;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

#if ILRuntime
namespace Model
{
    public static class ILHelper
    {
        public static void InitILRuntime(AppDomain appdomain)
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

            RegisterMethodDelegate<bool>(appdomain);
            RegisterMethodDelegate<int, object>(appdomain);
            RegisterMethodDelegate<ILTypeInstance, ILTypeInstance, ILTypeInstance, ILTypeInstance>(appdomain);

            #endregion

            RegisterMethodDelegate<ILTypeInstance>(appdomain);

            CLRBindings.Initialize(appdomain);

            #region appdomain.RegisterCrossBindingAdaptor

            var assembly = typeof(Init).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                var attrs = type.GetCustomAttributes(typeof(AdaptorAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                var obj = Activator.CreateInstance(type);
                if (!(obj is CrossBindingAdaptor adapt))
                {
                    continue;
                }

                appdomain.RegisterCrossBindingAdaptor(adapt);
            }

            #endregion

            #region appdomain.RegisterValueTypeBinder

            #endregion

            //LitJson注册
            JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
        }

        public static void RegisterMethodDelegate<T1>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1));
            if (m_1.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterMethodDelegate<{a}>");
            }
            else
            {
                m_1.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1>();
        }

        public static void RegisterMethodDelegate<T1, T2>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2));
            if (m_2.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterMethodDelegate<{a}>");
            }
            else
            {
                m_2.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1, T2>();
        }

        public static void RegisterMethodDelegate<T1, T2, T3>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3));
            if (m_3.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterMethodDelegate<{a}>");
            }
            else
            {
                m_3.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1, T2, T3>();
        }

        public static void RegisterMethodDelegate<T1, T2, T3, T4>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            if (m_4.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterMethodDelegate<{a}>");
            }
            else
            {
                m_4.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterMethodDelegate<T1, T2, T3, T4>();
        }

        public static void RegisterFunctionDelegate<TResult>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(TResult));
            if (f_0.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterFunctionDelegate<{a}>");
            }
            else
            {
                f_0.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<TResult>();
        }

        public static void RegisterFunctionDelegate<T1, TResult>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(TResult));
            if (f_1.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterFunctionDelegate<{a}>");
            }
            else
            {
                f_1.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, TResult>();
        }

        public static void RegisterFunctionDelegate<T1, T2, TResult>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(TResult));
            if (f_2.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterFunctionDelegate<{a}>");
            }
            else
            {
                f_2.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, T2, TResult>();
        }

        public static void RegisterFunctionDelegate<T1, T2, T3, TResult>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3), typeof(TResult));
            if (f_3.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterFunctionDelegate<{a}>");
            }
            else
            {
                f_3.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, T2, T3, TResult>();
        }

        public static void RegisterFunctionDelegate<T1, T2, T3, T4, TResult>(AppDomain appdomain)
        {
#if UNITY_EDITOR
            var a = new TClass(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(TResult));
            if (f_4.Contains(a))
            {
                Debug.LogError($"重复注册：RegisterFunctionDelegate<{a}>");
            }
            else
            {
                f_4.Add(a);
            }
#endif
            appdomain.DelegateManager.RegisterFunctionDelegate<T1, T2, T3, T4, TResult>();
        }

#if UNITY_EDITOR
        private static readonly List<TClass> m_1 = new List<TClass>();
        private static readonly List<TClass> m_2 = new List<TClass>();
        private static readonly List<TClass> m_3 = new List<TClass>();
        private static readonly List<TClass> m_4 = new List<TClass>();
        private static readonly List<TClass> f_0 = new List<TClass>();
        private static readonly List<TClass> f_1 = new List<TClass>();
        private static readonly List<TClass> f_2 = new List<TClass>();
        private static readonly List<TClass> f_3 = new List<TClass>();
        private static readonly List<TClass> f_4 = new List<TClass>();

        //防止重复注册
        private class TClass
        {
            private readonly List<Type> list = new List<Type>();

            public TClass(params Type[] types)
            {
                this.list.Clear();
                if (types == null)
                {
                    return;
                }

                for (var i = 0; i < types.Length; i++)
                {
                    this.list.Add(types[i]);
                }
            }

            private Type[] Types => this.list.ToArray();

            public override bool Equals(object obj)
            {
                if (obj is TClass s)
                {
                    var ts1 = s.Types;
                    var ts2 = this.Types;
                    if (ts1.Length != ts2.Length)
                    {
                        return false;
                    }

                    for (var i = 0; i < ts1.Length; i++)
                    {
                        if (ts1[i] != ts2[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                var str = "";
                for (var i = 0; i < this.list.Count; i++)
                {
                    str += $"{this.list[i].FullName}, ";
                }

                return str.Trim().Trim(',').Trim();
            }
        }
#endif
    }
}
#endif