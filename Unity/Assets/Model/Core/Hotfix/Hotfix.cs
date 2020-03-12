using System;
#if ILRuntime
using ILRuntime.Mono.Cecil.Pdb;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using System.IO;

#else
using System.Reflection;

#endif

namespace Model
{
    public sealed class Hotfix
    {
#if ILRuntime
        public AppDomain AppDomain { get; private set; }
#else
        private Assembly assembly;
#endif

        private IStaticMethod start;

        public Action Update;
        public Action LateUpdate;
        public Action OnApplicationQuit;
        public Action<bool> OnApplicationFocus;
        public Action<bool> OnApplicationPause;

        public Action<int, object> OnMessage;

        public void GotoHotfix()
        {
#if ILRuntime
            ILHelper.InitILRuntime(this.AppDomain);
#endif
            this.start.Run();
        }

        public void InitHotfixAssembly(byte[] assBytes, byte[] pdbBytes)
        {
#if ILRuntime
            Log.Debug("当前使用的是ILRuntime模式");
            this.AppDomain = new AppDomain();
            var dllStream = new MemoryStream(assBytes);
#if ILRuntime_Pdb
            var pdbStream = new MemoryStream(pdbBytes);
            this.AppDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());
#else
            appDomain.LoadAssembly(dllStream);
#endif
            this.start = new ILStaticMethod(this.AppDomain, "Hotfix.Init", "Start", 0);
#else
            Log.Debug("当前使用的是Mono模式");
            this.assembly = Assembly.Load(assBytes, pdbBytes);
            var hotfixInit = this.assembly.GetType("Hotfix.Init");
            this.start = new MonoStaticMethod(hotfixInit, "Start");
#endif
        }
    }
}