using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<KV> Data = new List<KV>();

        private readonly Dictionary<string, UnityEngine.Object> dic = new Dictionary<string, UnityEngine.Object>();

        public T Get<T>(string key) where T : UnityEngine.Object
        {
            if (!dic.TryGetValue(key, out UnityEngine.Object dictGo))
            {
                return null;
            }
            return dictGo as T;
        }

        public UnityEngine.Object GetObject(string key)
        {
            if (!dic.TryGetValue(key, out UnityEngine.Object dictGo))
            {
                return null;
            }
            return dictGo;
        }

        public void OnAfterDeserialize()
        {
            //发生在非主线程中
            dic.Clear();
            for (int i = 0; i < Data.Count; i++)
            {
                KV item = Data[i];
                if (string.IsNullOrEmpty(item.Key) || dic.ContainsKey(item.Key))
                {
                    continue;
                }
                dic.Add(item.Key, item.Value);
            }
        }

        public void OnBeforeSerialize() { }

        [Serializable]
        public class KV : IComparable
        {
            public string Key;
            public UnityEngine.Object Value;

            public int CompareTo(object obj)
            {
                if (obj is KV s)
                {
                    return Key.CompareTo(s.Key);
                }
                else
                {
                    return 1;
                }
            }
        }
    }

    internal static class ReferenceCollectorExtension
    {
        public static T Get<T>(this GameObject gameObject, string key) where T : UnityEngine.Object
        {
            try
            {
                return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
            }
            catch (Exception e)
            {
                throw new Exception($"获取{gameObject.name}的ReferenceCollector key失败, key: {key}", e);
            }
        }
    }
}