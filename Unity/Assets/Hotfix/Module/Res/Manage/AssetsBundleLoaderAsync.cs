using Model;
using System;
using UnityEngine;

namespace Hotfix
{
    internal sealed class AssetsBundleLoaderAsync : Component
    {
        private AssetBundleCreateRequest request;
        private TaskCompletionSource<AssetBundle> tcs;

        public Task<AssetBundle> LoadAsync(string path)
        {
            tcs = new TaskCompletionSource<AssetBundle>();
            request = AssetBundle.LoadFromFileAsync(path);
            request.completed += OnComplete;
            return tcs.Task;
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

        private void OnComplete(AsyncOperation obj)
        {
            if (request != null)
            {
                tcs.SetResult(request.assetBundle);
            }
            else
            {
                tcs.SetException(new Exception($"加载AB出错！"));
            }
        }
    }
}