using System;
using Async;
using UnityEngine;

namespace Hotfix
{
    internal class AssetsLoaderAsync : Component, IDestroySystem
    {
        private AssetBundleRequest request;
        private TaskCompletionSource<UnityEngine.Object[]> tcs;

        public async Task<UnityEngine.Object[]> LoadAllAssetsAsync(AssetBundle assetBundle)
        {
            await InnerLoadAllAssetsAsync(assetBundle);
            return this.request.allAssets;
        }

        public void Destroy()
        {
            this.request.completed -= OnComplete;
            this.request = null;
            this.tcs = null;
        }

        private Task InnerLoadAllAssetsAsync(AssetBundle assetBundle)
        {
            this.tcs = new TaskCompletionSource<UnityEngine.Object[]>();
            this.request = assetBundle.LoadAllAssetsAsync();
            this.request.completed += OnComplete;
            return this.tcs.Task;
        }

        private void OnComplete(AsyncOperation obj)
        {
            if (this.request != null && this.request.isDone)
            {
                this.tcs.SetResult(this.request.allAssets);
            }
            else
            {
                this.tcs.SetException(new Exception("异步加载AB里面的资源出错！"));
            }
        }
    }
}