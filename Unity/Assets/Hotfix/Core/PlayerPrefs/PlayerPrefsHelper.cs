using System;
using UnityEngine;

namespace Hotfix
{
    internal static class PlayerPrefsHelper
    {
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void DeleteKey(string key, int group = 0)
        {
            Log.Debug($"DeleteKey Key:{key} Group:{group}");
            key = CollectKey(key, group);
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }

        public static float GetFloat(string key, float defaultValue = 0f, int group = 0)
        {
            Log.Debug($"GetFloat Key:{key} Group:{group}");
            key = CollectKey(key, group);
            if (PlayerPrefs.HasKey(key))
            {
                var value = PlayerPrefs.GetFloat(key, defaultValue);
                Log.Debug($"value:{value}");
                return value;
            }

            Log.Debug($"value:{defaultValue}");
            return defaultValue;
        }

        public static int GetInt(string key, int defaultValue = 0, int group = 0)
        {
            Log.Debug($"GetInt Key:{key} Group:{group}");
            key = CollectKey(key, group);
            if (PlayerPrefs.HasKey(key))
            {
                var value = PlayerPrefs.GetInt(key, defaultValue);
                Log.Debug($"value:{value}");
                return value;
            }

            Log.Debug($"value:{defaultValue}");
            return defaultValue;
        }

        public static string GetString(string key, string defaultValue = null, int group = 0)
        {
            Log.Debug($"GetString Key:{key} Group:{group}");
            key = CollectKey(key, group);
            if (PlayerPrefs.HasKey(key))
            {
                var value = PlayerPrefs.GetString(key, defaultValue);
                Log.Debug($"value:{value}");
                return value;
            }

            Log.Debug($"value:{defaultValue}");
            return defaultValue;
        }

        public static bool GetBool(string key, bool defaultValue = false, int group = 0)
        {
            Log.Debug($"GetBool Key:{key} Group:{group}");
            key = CollectKey(key, group);
            if (PlayerPrefs.HasKey(key))
            {
                var value = PlayerPrefs.GetString(key, defaultValue ? "1" : "0") == "1";
                Log.Debug($"value:{value}");
                return value;
            }

            Log.Debug($"value:{defaultValue}");
            return defaultValue;
        }

        public static string GetJson(string key, string defaultValue = null, int group = 0)
        {
            return GetString(key, defaultValue, group);
        }

        public static T GetObject<T>(string key, T defaultValue, int group = 0)
        {
            var json = GetJson(key, null, group);
            if (string.IsNullOrEmpty(json))
            {
                return defaultValue;
            }

            try
            {
                return JsonHelper.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return defaultValue;
        }

        public static bool HasKey(string key, int group = 0)
        {
            Log.Debug($"HasKey Key:{key} Group:{group}");
            key = CollectKey(key, group);
            if (PlayerPrefs.HasKey(key))
            {
                Log.Debug($"value:{true}");
                return true;
            }

            Log.Debug($"value:{false}");
            return false;
        }

        public static void SetFloat(string key, float value, int group = 0)
        {
            Log.Debug($"SetFloat Key:{key} Value:{value} Group:{group}");
            PlayerPrefs.SetFloat(CollectKey(key, group), value);
        }

        public static void SetInt(string key, int value, int group = 0)
        {
            Log.Debug($"SetInt Key:{key} Value:{value} Group:{group}");
            PlayerPrefs.SetInt(CollectKey(key, group), value);
        }

        public static void SetString(string key, string value, int group = 0)
        {
            Log.Debug($"SetString Key:{key} Value:{value} Group:{group}");
            PlayerPrefs.SetString(CollectKey(key, group), value);
        }

        public static void SetBool(string key, bool value, int group = 0)
        {
            Log.Debug($"SetBool Key:{key} Value:{value} Group:{group}");
            PlayerPrefs.SetString(CollectKey(key, group), value ? "1" : "0");
        }

        public static void SetJson(string key, string value, int group = 0)
        {
            SetString(key, value, group);
        }

        public static void SetObject(string key, object value, int group = 0)
        {
            try
            {
                SetJson(key, JsonHelper.ToJson(value), group);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }

        private static string CollectKey(string key, int group = 0)
        {
            return $"{key}_{group}";
        }
    }
}