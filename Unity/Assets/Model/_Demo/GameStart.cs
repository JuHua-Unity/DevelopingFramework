using System;
using UnityEngine;

namespace Model
{
    internal sealed class GameStart
    {
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="text">启动游戏的配置文件</param>
        public void Start(TextAsset text)
        {
            LaunchOptions options = LitJson.JsonMapper.ToObject<LaunchOptions>(text.text);
            GameObject code = LoadCode(options.CodeABName.ToLower());
            byte[] assBytes = code.Get<TextAsset>("Hotfix.dll").bytes;
            byte[] pdbBytes = null;
#if ILRuntime && ILRuntime_Pdb
            pdbBytes = code.Get<TextAsset>("Hotfix.pdb").bytes;
#endif
            Game.Start(assBytes, pdbBytes);
        }

        private GameObject LoadCode(string fileName)
        {
            GameObject go = null;

            if (!EditorGetCodeGo(fileName, ref go))
            {
                string path = null;
                if (Application.isMobilePlatform)
                {
                    path = $"{Application.persistentDataPath}/{Application.productName}/{Define.ABsPathParent}/{fileName}";
                    if (!System.IO.File.Exists(path))
                    {
                        Log.Debug($"热更路径[{path}]下没有该文件！");
                        path = $"{Application.streamingAssetsPath}/{Define.ABsPathParent}/{fileName}";
                    }

                    if (!System.IO.File.Exists(path))
                    {
                        throw new Exception($"本地路径[{path}]下没有该文件！");
                    }
                }
                else
                {
                    path = $"{Application.streamingAssetsPath}/{Define.ABsPathParent}/{fileName}";
                    if (!System.IO.File.Exists(path))
                    {
                        throw new Exception($"本地路径[{path}]下没有该文件！");
                    }
                }

                Log.Debug($"Path：{path}");
                AssetBundle ab = AssetBundle.LoadFromFile(path);
                if (ab == null)
                {
                    throw new Exception($"{fileName}加载失败！");
                }

                go = ab.LoadAsset<GameObject>("Code");
            }

            if (go == null)
            {
                throw new Exception($"{fileName}加载失败！");
            }

            return go;
        }

        private bool EditorGetCodeGo(string fileName, ref GameObject go)
        {
#if DEFINE_LOCALRES && UNITY_EDITOR

            string[] paths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(fileName);
            if (paths == null || paths.Length < 1)
            {
                throw new Exception($"{fileName}不存在！");
            }

            if (paths.Length != 1)
            {
                throw new Exception($"{fileName}存在多个！");
            }

            go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(paths[0]);

            return true;
#else

            return false;

#endif
        }
    }
}