using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hotfix
{
    public static class AssetBundleMD5Helper //开放给Editor使用
    {
        private const string format = "x2";

        public static string FileMD5(string filePath)
        {
            byte[] retVal;
            using (var file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                retVal = md5.ComputeHash(file);
            }

            var sb = new StringBuilder();
            foreach (var b in retVal)
            {
                sb.Append(b.ToString(format));
            }

            return sb.ToString();
        }
    }
}