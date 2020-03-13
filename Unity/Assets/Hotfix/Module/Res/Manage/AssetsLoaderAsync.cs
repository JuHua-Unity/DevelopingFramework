using Async;
using System;
using UnityEngine;

namespace Hotfix
{
    internal class AssetsLoaderAsync : Component
    {
        private AssetBundleRequest request;
        private TaskCompletionSource<UnityEngine.Object[]> tcs;

        public async Task<UnityEngine.Object[]> LoadAllAssetsAsync(AssetBundle assetBundle)
        {
            await InnerLoadAllAssetsAsync(assetBundle);
            return request.allAssets;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            request.completed -= OnComplete;
            request = null;
            tcs = null;
        }

        private Task InnerLoadAllAssetsAsync(AssetBundle assetBundle)
        {
            tcs = new TaskCompletionSource<UnityEngine.Object[]>();
            request = assetBundle.LoadAllAssetsAsync();
            request.completed += OnComplete;
            return tcs.Task;
        }

        private void OnComplete(AsyncOperation obj)
        {
            if (request != null && request.isDone)
            {
                tcs.SetResult(request.allAssets);
            }
            else
            {
                tcs.SetException(new Exception($"异步加载AB里面的资源出错！"));
            }
        }
    }
}