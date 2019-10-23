using System.IO;

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
        }

        /// <summary>
        /// 创建文件
        /// 如果文件存在则先删除再创建
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void CreateFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Create(path).Close();
        }
    }
}