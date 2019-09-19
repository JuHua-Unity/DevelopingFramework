using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class Model_Hotfix_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Model.Hotfix);

            field = type.GetField("Update", flag);
            app.RegisterCLRFieldGetter(field, get_Update_0);
            app.RegisterCLRFieldSetter(field, set_Update_0);
            field = type.GetField("LateUpdate", flag);
            app.RegisterCLRFieldGetter(field, get_LateUpdate_1);
            app.RegisterCLRFieldSetter(field, set_LateUpdate_1);
            field = type.GetField("OnApplicationQuit", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationQuit_2);
            app.RegisterCLRFieldSetter(field, set_OnApplicationQuit_2);
            field = type.GetField("OnApplicationFocus", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationFocus_3);
            app.RegisterCLRFieldSetter(field, set_OnApplicationFocus_3);
            field = type.GetField("OnApplicationPause", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationPause_4);
            app.RegisterCLRFieldSetter(field, set_OnApplicationPause_4);


        }



        static object get_Update_0(ref object o)
        {
            return ((Model.Hotfix)o).Update;
        }
        static void set_Update_0(ref object o, object v)
        {
            ((Model.Hotfix)o).Update = (System.Action)v;
        }
        static object get_LateUpdate_1(ref object o)
        {
            return ((Model.Hotfix)o).LateUpdate;
        }
        static void set_LateUpdate_1(ref object o, object v)
        {
            ((Model.Hotfix)o).LateUpdate = (System.Action)v;
        }
        static object get_OnApplicationQuit_2(ref object o)
        {
            return ((Model.Hotfix)o).OnApplicationQuit;
        }
        static void set_OnApplicationQuit_2(ref object o, object v)
        {
            ((Model.Hotfix)o).OnApplicationQuit = (System.Action)v;
        }
        static object get_OnApplicationFocus_3(ref object o)
        {
            return ((Model.Hotfix)o).OnApplicationFocus;
        }
        static void set_OnApplicationFocus_3(ref object o, object v)
        {
            ((Model.Hotfix)o).OnApplicationFocus = (System.Action<System.Boolean>)v;
        }
        static object get_OnApplicationPause_4(ref object o)
        {
            return ((Model.Hotfix)o).OnApplicationPause;
        }
        static void set_OnApplicationPause_4(ref object o, object v)
        {
            ((Model.Hotfix)o).OnApplicationPause = (System.Action<System.Boolean>)v;
        }


    }
}
