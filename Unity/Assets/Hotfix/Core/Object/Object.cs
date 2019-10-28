namespace Hotfix
{
    /// <summary>
    /// 万物皆对象
    /// </summary>
    internal class Object : ISupportInitialize, IDisposable
    {
        /// <summary>
        /// 构造函数
        /// 生成ObjId
        /// </summary>
        public Object()
        {
            ObjId = GenerateId();
        }

        /// <summary>
        /// 对象ID
        /// </summary>
        public long ObjId { get; private set; } = 0;

#if UNITY_EDITOR && ComponentView

        /// <summary>
        /// 对象名字
        /// </summary>
        public string ObjName { get; set; } = string.Empty;

#endif

        private bool isFromPool;//是否来自对象池

        /// <summary>
        /// 设置或者获取是否来自对象池
        /// </summary>
        public bool IsFromPool
        {
            get
            {
                return isFromPool;
            }
            set
            {
                isFromPool = value;

                if (!isFromPool)
                {
                    return;
                }

                if (ObjId == 0)
                {
                    ObjId = GenerateId();
                }
            }
        }

        /// <summary>
        /// 获取是否已经被释放
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return ObjId == 0;
            }
        }

        #region 接口实现

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            ObjId = 0;
        }

        /// <summary>
        /// 完成序列化
        /// </summary>
        public virtual void EndInit() { }

        #endregion

        #region Override

        public override string ToString()
        {
            return JsonHelper.ToJson(this);
        }

        #endregion

        private static long id;//所有实例id累积

        /// <summary>
        /// 生成ObjId
        /// </summary>
        /// <returns>ID</returns>
        private static long GenerateId()
        {
            return ++id;
        }
    }
}