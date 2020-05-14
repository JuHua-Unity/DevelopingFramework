namespace Hotfix
{
    internal partial class Component
    {
        protected static void DeleteAll()
        {
            PlayerPrefsHelper.DeleteAll();
        }

        protected static void DeleteKey(string key, int group = 0)
        {
            PlayerPrefsHelper.DeleteKey(key, group);
        }

        protected static float GetFloat(string key, float defaultValue = 0f, int group = 0)
        {
            return PlayerPrefsHelper.GetFloat(key, defaultValue, group);
        }

        protected static int GetInt(string key, int defaultValue = 0, int group = 0)
        {
            return PlayerPrefsHelper.GetInt(key, defaultValue, group);
        }

        protected static string GetString(string key, string defaultValue = null, int group = 0)
        {
            return PlayerPrefsHelper.GetString(key, defaultValue, group);
        }

        protected static bool GetBool(string key, bool defaultValue = false, int group = 0)
        {
            return PlayerPrefsHelper.GetBool(key, defaultValue, group);
        }

        protected static string GetJson(string key, string defaultValue = null, int group = 0)
        {
            return PlayerPrefsHelper.GetJson(key, defaultValue, group);
        }

        protected static T GetObject<T>(string key, T defaultValue, int group = 0)
        {
            return PlayerPrefsHelper.GetObject(key, defaultValue, group);
        }

        protected static bool HasKey(string key, int group = 0)
        {
            return PlayerPrefsHelper.HasKey(key, group);
        }

        protected static void SetFloat(string key, float value, int group = 0)
        {
            PlayerPrefsHelper.SetFloat(key, value, group);
        }

        protected static void SetInt(string key, int value, int group = 0)
        {
            PlayerPrefsHelper.SetInt(key, value, group);
        }

        protected static void SetString(string key, string value, int group = 0)
        {
            PlayerPrefsHelper.SetString(key, value, group);
        }

        protected static void SetBool(string key, bool value, int group = 0)
        {
            PlayerPrefsHelper.SetBool(key, value, group);
        }

        protected static void SetJson(string key, string value, int group = 0)
        {
            PlayerPrefsHelper.SetJson(key, value, group);
        }

        protected static void SetObject(string key, object value, int group = 0)
        {
            PlayerPrefsHelper.SetObject(key, value, group);
        }

        protected static void Save()
        {
            PlayerPrefsHelper.Save();
        }
    }
}