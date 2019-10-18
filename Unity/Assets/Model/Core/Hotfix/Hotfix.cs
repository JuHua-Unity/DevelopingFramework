using System;
#if ILRuntime
using System.IO;
#else
using System.Reflection;
#endif

namespace Model
{
    public sealed class Hotfix
    {
#if ILRuntime
        private ILRuntime.Runtime.Enviorment.AppDomain appDomain;
#else
        private Assembly assembly;
#endif

        private IStaticMethod start;

        public Action Update;
        public Action LateUpdate;
        public Action OnApplicationQuit;
        public Action<bool> OnApplicationFocus;
        public Action<bool> OnApplicationPause;

        public void GotoHotfix()
        {
#if ILRuntime
            ILHelper.InitILRuntime(appDomain);
#endif
            start.Run();
        }

        public void InitHotfixAssembly(byte[] assBytes, byte[] pdbBytes)
        {
#if ILRuntime
            Log.Debug($"当前使用的是ILRuntime模式");
            appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            var dllStream = new MemoryStream(assBytes);
#if ILRuntime_Pdb
            var pdbStream = new MemoryStream(pdbBytes);
            appDomain.LoadAssembly(dllStream, pdbStream, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#else
            appDomain.LoadAssembly(dllStream);
#endif
            start = new ILStaticMethod(appDomain, "Hotfix.Init", "Start", 0);
#else
            Log.Debug($"当前使用的是Mono模式");
            assembly = Assembly.Load(assBytes, pdbBytes);
            Type hotfixInit = assembly.GetType("Hotfix.Init");
            start = new MonoStaticMethod(hotfixInit, "Start");
#endif
        }
    }
}