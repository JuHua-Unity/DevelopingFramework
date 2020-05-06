using UnityEngine;
#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR
using ObjectDrawer;

#endif

namespace Hotfix
{
    /// <inheritdoc cref="ISupportInitialize" />
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// 万物皆对象
    /// </summary>
    internal class Object : ISupportInitialize, IDisposable
    {
        public static GameObject GameRoot { get; set; }

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR

        public static GameObject DisposedObjectsParent { get; set; }
        public static GameObject ObjectsParent { get; set; }

        public GameObject GameObject { get; private set; }

        /// <summary>
        /// 对象名字
        /// </summary>
        public string ObjName { get; set; }

        public void SetParent(GameObject go)
        {
            this.GameObject.transform.SetParent(go == null ? DisposedObjectsParent.transform : go.transform, false);
        }

#endif
        /// <summary>
        /// 构造函数
        /// 生成ObjId
        /// </summary>
        public Object()
        {
            this.ObjId = GenerateId();

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR

            this.ObjName = GetType().Name;

            if (this.GameObject != null)
            {
                return;
            }

            this.GameObject = new GameObject(this.ObjName);
            SetParent(ObjectsParent);
            this.GameObject.AddComponent<ObjectView>().Obj = this;

#endif

            Log.Debug($"Create->ID:{this.ObjId} Name:{GetType().FullName}");
        }

        /// <summary>
        /// 对象ID
        /// </summary>
        public ulong ObjId { get; private set; }

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

            if (this.IsFromPool)
            {
                Game.ObjectPool.Recycle(this);

#if UNITY_EDITOR && !ILRuntime && ObjectView && DEFINE_HOTFIXEDITOR
                SetParent(DisposedObjectsParent);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(this.GameObject);
                this.GameObject = null;
#endif
            }
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