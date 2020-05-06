using LitJson;

namespace Model
{
    /// <summary>
    /// Json辅助器
    /// </summary>
    internal static class JsonHelper
    {
        public static string ToJson(object obj)
        {
            switch (obj)
            {
                case string s:
                    return s;
                case int _:
                case float _:
                case bool _:
                case long _:
                case double _:
                case byte _:
                case char _:
                case decimal _:
                case sbyte _:
                case short _:
                case uint _:
                case ulong _:
                case ushort _:
                    return obj.ToString();
                default:
                    return JsonMapper.ToJson(obj);
            }
        }

        public static T FromJson<T>(string str)
        {
            return JsonMapper.ToObject<T>(str);
        }
    }
}