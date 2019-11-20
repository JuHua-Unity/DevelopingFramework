using Model;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Hotfix
{
    internal sealed class UnityWebRequestAsync : Component
    {
        private static readonly AcceptAllCertificate certificateHandler = new AcceptAllCertificate();

        private UnityWebRequest request;
        private TaskCompletionSource<DownloadHandler> tcs;
        private UnityWebRequestAsyncOperation asyncOperation;

        /// <summary>
        /// 异步下载
        /// 下载前请确保当前没有在下载中！
        /// </summary>
        /// <param name="url">下载URL</param>
        /// <returns></returns>
        public Task<DownloadHandler> DownloadAsync(string url)
        {
            tcs = new TaskCompletionSource<DownloadHandler>();

            url = url.Replace(" ", "%20");
            request = UnityWebRequest.Get(url);
            request.certificateHandler = certificateHandler;
            asyncOperation = request.SendWebRequest();
            asyncOperation.completed += OnComplete;

            return tcs.Task;
        }

        public ulong Bytes
        {
            get
            {
                if (request == null)
                {
                    return 0;
                }

                return request.downloadedBytes;
            }
        }

        public bool Downloading
        {
            get
            {
                return request != null;
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            if (asyncOperation != null && !asyncOperation.isDone)
            {
                request.Abort();
            }

            asyncOperation.completed -= OnComplete;
            asyncOperation = null;

            request.Dispose();
            request = null;

            tcs = null;
        }

        private void OnComplete(AsyncOperation obj)
        {
            EndRequest();
        }

        private void EndRequest()
        {
            if (request.isHttpError || request.isNetworkError)
            {
                tcs.SetException(new Exception($"UnityWebRequest 下载失败！"));
            }
            else
            {
                tcs.SetResult(request.downloadHandler);
            }
        }

        private class AcceptAllCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }
    }
}