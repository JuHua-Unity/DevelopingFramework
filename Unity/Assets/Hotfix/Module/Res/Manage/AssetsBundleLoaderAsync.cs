using System;
using Async;
using UnityEngine;

namespace Hotfix
{
    internal sealed class AssetsBundleLoaderAsync : Component, IDestroySystem
    {
        private AssetBundleCreateRequest request;
        private TaskCompletionSource<AssetBundle> tcs;

        public Task<AssetBundle> LoadAsync(string path)
        {
            this.tcs = new TaskCompletionSource<AssetBundle>();
            this.request = AssetBundle.LoadFromFileAsync(path);
            this.request.completed += OnComplete;
            return this.tcs.Task;
        }

        public void Destroy()
        {
            this.request.completed -= OnComplete;
            this.request = null;
            this.tcs = null;
        }

        private void OnComplete(AsyncOperation obj)
        {
            if (this.request != null)
            {
                this.tcs.SetResult(this.request.assetBundle);
            }
            else
            {
                this.tcs.SetException(new Exception("加载AB出错！"));
            }
        }
    }
}