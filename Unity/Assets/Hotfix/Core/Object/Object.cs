namespace Hotfix
{
    /// <inheritdoc cref="ISupportInitialize" />
    /// <inheritdoc cref="IDisposable" />
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
            this.ObjId = GenerateId();
        }

        /// <summary>
        /// 对象ID
        /// </summary>
        public ulong ObjId { get; private set; }

#if UNITY_EDITOR && !ILRuntime && ComponentView

        /// <summary>
        /// 对象名字
        /// </summary>
        public string ObjName { get; set; } = string.Empty;

#endif

        private bool isFromPool; //是否来自对象池

        /// <summary>
        /// 设置或者获取是否来自对象池
        /// 注意：框架外不可设置
        /// </summary>
        public bool IsFromPool
        {
            get => this.isFromPool;
            set
            {
                this.isFromPool = value;

                if (!this.isFromPool)
                {
                    return;
                }

                if (this.ObjId == 0)
                {
                    this.ObjId = GenerateId();
                }
            }
        }

        /// <summary>
        /// 获取是否已经被释放
        /// </summary>
        public bool IsDisposed => this.ObjId == 0;

        #region 接口实现

        /// <inheritdoc />
        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.ObjId = 0;
        }

        /// <inheritdoc />
        /// <summary>
        /// 如果该对象支持序列化
        /// 完成序列化调用发生在JsonHelper.FromJson之后
        /// </summary>
        public virtual void EndInit()
        {
        }

        #endregion

        #region Override

        /// <summary>
        /// 重载ToString展示为Json
        /// </summary>
        /// <returns>json</returns>
        public override string ToString()
        {
            return JsonHelper.ToJson(this);
        }

        #endregion

        private static ulong id; //所有实例id累积

        /// <summary>
        /// 生成ObjId
        /// </summary>
        /// <returns>ID</returns>
        private static ulong GenerateId()
        {
            id++;
            return id;
        }
    }
}