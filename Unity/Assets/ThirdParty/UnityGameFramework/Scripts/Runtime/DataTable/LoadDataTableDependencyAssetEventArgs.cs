﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using System;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 加载数据表时加载依赖资源事件。
    /// </summary>
    public sealed class LoadDataTableDependencyAssetEventArgs : GameEventArgs
    {
        /// <summary>
        /// 加载数据表时加载依赖资源事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoadDataTableDependencyAssetEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载数据表时加载依赖资源事件的新实例。
        /// </summary>
        public LoadDataTableDependencyAssetEventArgs()
        {
            DataRowType = null;
            DataTableName = null;
            DataTableAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取加载数据表时加载依赖资源事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        public Type DataRowType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        public string DataTableName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数据表资源名称。
        /// </summary>
        public string DataTableAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载数据表时加载依赖资源事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载数据表时加载依赖资源事件。</returns>
        public static LoadDataTableDependencyAssetEventArgs Create(GameFramework.DataTable.LoadDataTableDependencyAssetEventArgs e)
        {
            LoadDataTableInfo loadDataTableInfo = (LoadDataTableInfo)e.UserData;
            LoadDataTableDependencyAssetEventArgs loadDataTableDependencyAssetEventArgs = ReferencePool.Acquire<LoadDataTableDependencyAssetEventArgs>();
            loadDataTableDependencyAssetEventArgs.DataRowType = loadDataTableInfo.DataRowType;
            loadDataTableDependencyAssetEventArgs.DataTableName = loadDataTableInfo.DataTableName;
            loadDataTableDependencyAssetEventArgs.DataTableAssetName = e.DataTableAssetName;
            loadDataTableDependencyAssetEventArgs.DependencyAssetName = e.DependencyAssetName;
            loadDataTableDependencyAssetEventArgs.LoadedCount = e.LoadedCount;
            loadDataTableDependencyAssetEventArgs.TotalCount = e.TotalCount;
            loadDataTableDependencyAssetEventArgs.UserData = loadDataTableInfo.UserData;
            return loadDataTableDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载数据表时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            DataRowType = null;
            DataTableName = null;
            DataTableAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
