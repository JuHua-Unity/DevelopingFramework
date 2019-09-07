namespace Model
{
    internal class ILStaticMethod : IStaticMethod
    {
        private readonly ILRuntime.Runtime.Enviorment.AppDomain appDomain;
        private readonly ILRuntime.CLR.Method.IMethod method;
        private readonly object[] param;

        public ILStaticMethod(ILRuntime.Runtime.Enviorment.AppDomain appDomain, string typeName, string methodName, int paramsCount)
        {
            this.appDomain = appDomain;
            method = appDomain.GetType(typeName).GetMethod(methodName, paramsCount);
            param = new object[paramsCount];
        }

        public override void Run()
        {
            appDomain.Invoke(method, null, param);
        }

        public override void Run(object a)
        {
            param[0] = a;
            appDomain.Invoke(method, null, param);
        }

        public override void Run(object a, object b)
        {
            param[0] = a;
            param[1] = b;
            appDomain.Invoke(method, null, param);
        }

        public override void Run(object a, object b, object c)
        {
            param[0] = a;
            param[1] = b;
            param[2] = c;
            appDomain.Invoke(method, null, param);
        }
    }
}