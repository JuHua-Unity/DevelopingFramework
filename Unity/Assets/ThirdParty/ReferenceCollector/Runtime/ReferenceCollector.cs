using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ReferenceCollector
{
    public class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<KV> Data = new List<KV>();

        private readonly Dictionary<string, Object> dic = new Dictionary<string, Object>();

        public T Get<T>(string key) where T : Object
        {
            if (!this.dic.TryGetValue(key, out var dictGo))
            {
                return null;
            }

            return dictGo as T;
        }

        public Object GetObject(string key)
        {
            if (!this.dic.TryGetValue(key, out var dictGo))
            {
                return null;
            }

            return dictGo;
        }

        public void OnAfterDeserialize()
        {
            //发生在非主线程中
            this.dic.Clear();
            for (var i = 0; i < this.Data.Count; i++)
            {
                var item = this.Data[i];
                if (string.IsNullOrEmpty(item.Key) || this.dic.ContainsKey(item.Key))
                {
                    continue;
                }

                this.dic.Add(item.Key, item.Value);
            }
        }

        public void OnBeforeSerialize()
        {
        }

        [Serializable]
        public class KV : IComparable
        {
            public string Key;
            public Object Value;

            public int CompareTo(object obj)
            {
                if (obj is KV s)
                {
                    return string.Compare(this.Key, s.Key, StringComparison.Ordinal);
                }

                return 1;
            }
        }
    }

    public static class ReferenceCollectorExtension
    {
        public static T Get<T>(this GameObject gameObject, string key) where T : Object
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