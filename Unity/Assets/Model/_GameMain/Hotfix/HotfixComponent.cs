using System;
using System.IO;
using System.Reflection;
using ILRuntime.Mono.Cecil.Pdb;
using UnityEngine;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace Model
{
    /// <summary>
    /// 热更新组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Hotfix")]
    public sealed class HotfixComponent : GameFrameworkComponent
    {
        public  AppDomain AppDomain { get; private set; }
        private Assembly  assembly;

        private IStaticMethod start;

        public Action       UpdateAction;
        public Action       LateUpdateAction;
        public Action       OnApplicationQuitAction;
        public Action<bool> OnApplicationFocusAction;
        public Action<bool> OnApplicationPauseAction;

        public Action<int, object> OnMessage;

        [SerializeField] private bool useILRuntime;
        [SerializeField] private bool usePdb;

        /// <summary>
        /// 获取或者设置使用ILRuntime进行热更新
        /// </summary>
        public bool UseILRuntime
        {
            get => this.useILRuntime;
            set => this.useILRuntime = value;
        }

        /// <summary>
        /// 获取或者设置热更新使用Pdb
        /// </summary>
        public bool UsePdb
        {
            get => this.usePdb;
            set => this.usePdb = value;
        }

        public void GotoHotfix()
        {
            this.start.Run();
        }

        public void InitAssembly(byte[] assBytes, byte[] pdbBytes)
        {
            if (this.UseILRuntime)
            {
                if (this.UsePdb)
                {
                    InitAssembly_IL(assBytes, pdbBytes);
                }
                else
                {
                    InitAssembly_IL(assBytes);
                }
            }
            else
            {
                InitAssembly_Mono(assBytes, pdbBytes);
            }
        }

        private void InitAssembly_IL(byte[] assBytes, byte[] pdbBytes)
        {
            this.AppDomain = new AppDomain();
            this.AppDomain.LoadAssembly(new MemoryStream(assBytes), new MemoryStream(pdbBytes), new PdbReaderProvider());

            this.start = new ILStaticMethod(this.AppDomain, "Hotfix.Init", "Start", 0);
        }

        private void InitAssembly_IL(byte[] assBytes)
        {
            this.AppDomain = new AppDomain();
            this.AppDomain.LoadAssembly(new MemoryStream(assBytes));

            this.start = new ILStaticMethod(this.AppDomain, "Hotfix.Init", "Start", 0);
        }

        private void InitAssembly_Mono(byte[] assBytes, byte[] pdbBytes)
        {
            this.assembly = Assembly.Load(assBytes, pdbBytes);

            this.start = new MonoStaticMethod(this.assembly.GetType("Hotfix.Init"), "Start");
        }

        #region 系统周期

        private void Update()
        {
            this.UpdateAction?.Invoke();
        }

        private void LateUpdate()
        {
            this.LateUpdateAction?.Invoke();
        }

        private void OnApplicationFocus(bool focus)
        {
            this.OnApplicationFocusAction?.Invoke(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            this.OnApplicationPauseAction?.Invoke(pause);
        }

        private void OnApplicationQuit()
        {
            this.OnApplicationQuitAction?.Invoke();
        }

        #endregion
    }
}