using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hotfix
{
    public static class MD5Helper
    {
        public static string FileMD5(string filePath)
        {
            byte[] retVal;
            using (var file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                retVal = md5.ComputeHash(file);
            }

            const string format = "x2";
            var stringBuilder = new StringBuilder();
            foreach (var b in retVal)
            {
                stringBuilder.Append(b.ToString(format));
            }

            return stringBuilder.ToString();
        }
    }
}