using System;
using System.IO;
using UnityEditor;

namespace Editors
{
    internal class IOHelper
    {
        public static void StreamWriter(string data, string path)
        {
            CreateFile(path);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(data);
            }

            AssetDatabase.Refresh();
        }

        public static T StreamReader<T>(string path)
        {
            if (File.Exists(path))
            {
                string str = "";
                using (StreamReader sb = new StreamReader(path))
                {
                    str = sb.ReadToEnd();
                }

                return LitJson.JsonMapper.ToObject<T>(str);
            }
            else
            {
                var t = typeof(T);
                if (t == typeof(string))
                {
                    throw new Exception($"直接获取字符串类型的话请使用非泛型方法");
                }

                return (T)Activator.CreateInstance(t);
            }
        }

        public static string StreamReader(string path)
        {
            if (File.Exists(path))
            {
                string str = "";
                using (StreamReader sb = new StreamReader(path))
                {
                    str = sb.ReadToEnd();
                }

                return str;
            }
            else
            {
                return string.Empty;
            }
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