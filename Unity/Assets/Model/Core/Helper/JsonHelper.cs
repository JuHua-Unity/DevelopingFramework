namespace Model
{
    /// <summary>
    /// Json辅助器
    /// </summary>
    internal static class JsonHelper
    {
        public static string ToJson(object obj)
        {
            if (obj is string s)
            {
                return s;
            }

            if (obj is int
                || obj is float
                || obj is bool
                || obj is long
                || obj is double
                || obj is byte
                || obj is char
                || obj is decimal
                || obj is sbyte
                || obj is short
                || obj is uint
                || obj is ulong
                || obj is ushort)
            {
                return obj.ToString();
            }

            return LitJson.JsonMapper.ToJson(obj);
        }

        public static T FromJson<T>(string str)
        {
            return LitJson.JsonMapper.ToObject<T>(str);
        }
    }
}