namespace Model
{
    /// <summary>
    /// 支持序列化接口
    /// </summary>
    internal interface ISupportInitialize
    {
        /// <summary>
        /// 序列化完成后调用
        /// </summary>
        void EndInit();
    }
}