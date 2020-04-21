using LitJson;

namespace Hotfix
{
    /// <summary>
    /// Json辅助器
    /// </summary>
    internal static class JsonHelper
    {
        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <returns></returns>
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

        /// <summary>
        /// 反序列化为对象
        /// 如果该对象支持序列化(即：继承了ISupportInitialize接口)将会调用EndInit方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="str">字符串</param>
        /// <returns>该对象</returns>
        public static T FromJson<T>(string str)
        {
            var t = JsonMapper.ToObject<T>(str);
            if (!(t is ISupportInitialize iSupportInitialize))
            {
                return t;
            }

            iSupportInitialize.EndInit();
            return t;
        }
    }
}