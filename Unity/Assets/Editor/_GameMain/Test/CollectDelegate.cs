using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class CollectDelegate : Editor
    {
        public static void FindDelegate()
        {
            //CheckDelegate(typeof(Model.Define).Assembly);
            CheckDelegate(typeof(Test.Test).Assembly);
            Save();
        }

        private static Dictionary<Type, string> types = new Dictionary<Type, string>();
        private static Dictionary<Type, string> names = new Dictionary<Type, string>();
        private static List<string> methodStrs = new List<string>();

        private static void CheckDelegate(Assembly assembly)
        {
            int count = names.Count;
            Debug.Log($"开始-{assembly.FullName}");

            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (!(types[i].IsPublic || types[i].IsNestedPublic))
                {
                    continue;
                }

                if (types[i] == typeof(Model.CollectDelegate))
                {
                    continue;
                }

                var objs = types[i].GetCustomAttributes(typeof(ObsoleteAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    continue;
                }

                List<Type> gs = new List<Type>();
                if (types[i].IsGenericType)
                {
                    gs.AddRange(types[i].GetGenericArguments());
                }

                FieldInfo[] fields = types[i].GetFields();
                for (int j = 0; j < fields.Length; j++)
                {
                    if (PreCollect(fields[j].FieldType, gs))
                    {
                        Collect(fields[j].FieldType);
                    }
                    else
                    {
                        methodStrs.Add($"类型：{types[i].FullName} 字段：{fields[j].Name}");
                    }
                }

                MethodInfo[] methods = types[i].GetMethods();
                for (int j = 0; j < methods.Length; j++)
                {
                    if (methods[j].IsGenericMethod)
                    {
                        gs.AddRange(methods[j].GetGenericArguments());
                    }

                    List<ParameterInfo> parameters = new List<ParameterInfo>();
                    parameters.AddRange(methods[j].GetParameters());
                    parameters.Add(methods[j].ReturnParameter);
                    for (int k = 0; k < parameters.Count; k++)
                    {
                        if (PreCollect(parameters[k].ParameterType, gs))
                        {
                            Collect(parameters[k].ParameterType);
                        }
                        else
                        {
                            methodStrs.Add($"类型：{types[i].FullName} 方法：{methods[j].Name}");
                        }
                    }
                }
            }

            Debug.Log($"结束-{names.Count - count}-{assembly.FullName}");
        }

        private static bool PreCollect(Type type, List<Type> gs)
        {
            if (IsDelegate(type) && type.IsGenericType)
            {
                Type[] ts = type.GetGenericArguments();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (gs.Contains(ts[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void Save()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-----Model-----");
            foreach (var item in types)
            {
                sb.AppendLine($"public {item.Value} {names[item.Key]};");
            }

            sb.AppendLine("-----Hotfix-----");
            foreach (var item in names)
            {
                string s = "try{ new Model.CollectDelegate()." + $"{item.Value} = {item.Value}" + "; }catch (Exception e) { sb.AppendLine(e.Message); }";
                sb.AppendLine(s);
            }

            sb.AppendLine("-----泛型方法-----");
            for (int i = 0; i < methodStrs.Count; i++)
            {
                sb.AppendLine($"{methodStrs[i]}");
            }

            SelectPath("CollectDelegate.txt", sb.ToString());
        }

        private static void SelectPath(string name, string str)
        {
            string path = EditorUtility.OpenFolderPanel("保存路径", "", "");
            if (Directory.Exists(path))
            {
                Save(Path.Combine(path, name), str);
            }
            else
            {
                SelectPath(name, str);
            }
        }

        private static void Save(string path, string str)
        {
            Debug.Log(str);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Close();

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(str);
            }
        }

        private static void Collect(Type type)
        {
            if (IsDelegate(type))
            {
                if (type.IsGenericType && type.GetGenericArguments().Length > 4)
                {
                    return;
                }

                string t = GetTypeStr(type);
                string n = GetNameStr(type);

                if (t.StartsWith("ILRuntime") ||
                    t.StartsWith("Mono.Cecil") ||
                    t.StartsWith("LitJson"))
                {
                    return;
                }

                int index = 0;
                while (names.ContainsValue(n))
                {
                    n = $"{n}{index}";
                    index++;
                }

                types[type] = t;
                names[type] = n;
            }
        }

        private static string GetTypeStr(Type type)
        {
            string str = string.IsNullOrEmpty(type.FullName) ? type.Name : type.FullName;
            str = str.Replace("+", ".");
            if (type.IsGenericType)
            {
                Type[] ts = type.GetGenericArguments();
                List<string> names = new List<string>();
                for (int i = 0; i < ts.Length; i++)
                {
                    names.Add(GetTypeStr(ts[i]));
                }

                int len = (string.IsNullOrEmpty(type.FullName) ? type.Name : type.FullName).IndexOf('`');
                str = str.Substring(0, len);

                return $"{str}<{string.Join(",", names)}>";
            }
            else
            {
                return str;
            }
        }

        private static string GetNameStr(Type type)
        {
            string str = type.Name;
            str = str.Replace("[]", "s");
            if (type.IsGenericType)
            {
                Type[] ts = type.GetGenericArguments();
                List<string> names = new List<string>();
                for (int i = 0; i < ts.Length; i++)
                {
                    names.Add(GetNameStr(ts[i]));
                }

                int len = type.Name.IndexOf('`');
                str = str.Substring(0, len);

                return $"M_{str}{string.Join("", names)}";
            }
            else
            {
                return $"M_{str}";
            }
        }

        private static bool IsDelegate(Type type)
        {
            if (type == typeof(Delegate))
            {
                return true;
            }

            if (type.IsSubclassOf(typeof(Delegate)))
            {
                return true;
            }

            return typeof(Delegate).IsAssignableFrom(type);
        }
    }
}