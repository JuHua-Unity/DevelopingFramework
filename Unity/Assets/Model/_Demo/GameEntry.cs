//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Threading;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Model
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            Log.Debug($"UnityVersion:{Application.unityVersion}\tAppVersion:{Application.version}\t游戏名:{Application.productName}\tPlatform:{Application.platform}\tThreadId:{Thread.CurrentThread.ManagedThreadId}");
        }

        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}