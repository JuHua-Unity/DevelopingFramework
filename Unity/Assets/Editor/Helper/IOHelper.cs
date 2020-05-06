using System;
using System.IO;
using LitJson;
using UnityEditor;

namespace Editors
{
    internal class IOHelper
    {
        public static void StreamWriter(string data, string path)
        {
            CreateFile(path);
            using (var sw = new StreamWriter(path))
            {
                sw.Write(data);
            }

            AssetDatabase.Refresh();
        }

        public static T StreamReader<T>(string path)
        {
            var t = typeof(T);
            if (t == typeof(string))
            {
                throw new Exception("直接获取字符串类型的话请使用非泛型方法");
            }

            var str = StreamReader(path);
            if (!string.IsNullOrEmpty(str))
            {
                return JsonMapper.ToObject<T>(str);
            }

            return (T) Activator.CreateInstance(t);
        }

        public static string StreamReader(string path)
        {
            if (!File.Exists(path))
            {
                return string.Empty;
            }

            string str;
            using (var sb = new StreamReader(path))
            {
                str = sb.ReadToEnd();
            }

            return str;
        }

        /// <summary>
        /// 创建文件
        /// 如果文件存在则先删除再创建
        /// </summary>
        /// <param name="path">文件路径</param>
        private static void CreateFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Close();
        }
    }
}