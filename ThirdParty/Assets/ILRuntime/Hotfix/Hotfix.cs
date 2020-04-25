using System;
#if ILRuntime
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using System.IO;

#else
using System.Reflection;

#endif

namespace ILRuntime.Hotfix
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
            this.start.Run();
        }

        public void InitHotfixAssembly(byte[] assBytes, byte[] pdbBytes)
        {
#if ILRuntime
            this.AppDomain = new AppDomain();
            var dllStream = new MemoryStream(assBytes);
#if ILRuntime_Pdb
            var pdbStream = new MemoryStream(pdbBytes);
            this.AppDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());
#else
            this.AppDomain.LoadAssembly(dllStream);
#endif
            this.start = new ILStaticMethod(this.AppDomain, "Hotfix.Init", "Start", 0);
#else
            this.assembly = Assembly.Load(assBytes, pdbBytes);
            var hotfixInit = this.assembly.GetType("Hotfix.Init");
            this.start = new MonoStaticMethod(hotfixInit, "Start");
#endif
        }
    }
}