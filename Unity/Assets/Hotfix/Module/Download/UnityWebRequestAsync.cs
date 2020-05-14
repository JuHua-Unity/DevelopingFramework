using System;
using Async;
using UnityEngine;
using UnityEngine.Networking;

namespace Hotfix
{
    internal sealed class UnityWebRequestAsync : Component, IDestroySystem
    {
        //private static readonly AcceptAllCertificate certificateHandler = new AcceptAllCertificate();

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
            this.tcs = new TaskCompletionSource<DownloadHandler>();

            url = url.Replace(" ", "%20");
            this.request = UnityWebRequest.Get(url);
            //request.certificateHandler = certificateHandler;
            this.asyncOperation = this.request.SendWebRequest();
            this.asyncOperation.completed += OnComplete;

            return this.tcs.Task;
        }

        public ulong Bytes => this.request?.downloadedBytes ?? 0;

        public bool Downloading => this.request != null;

        public void Destroy()
        {
            if (this.asyncOperation != null)
            {
                if (!this.asyncOperation.isDone)
                {
                    this.request.Abort();
                }

                this.asyncOperation.completed -= OnComplete;
            }

            this.asyncOperation = null;
            this.request?.Dispose();
            this.request = null;

            this.tcs = null;
        }

        private void OnComplete(AsyncOperation obj)
        {
            EndRequest();
        }

        private void EndRequest()
        {
            if (this.request.isHttpError || this.request.isNetworkError)
            {
                this.tcs.SetException(new Exception("UnityWebRequest 下载失败！"));
            }
            else
            {
                this.tcs.SetResult(this.request.downloadHandler);
            }
        }

        //private class AcceptAllCertificate : CertificateHandler
        //{
        //    protected override bool ValidateCertificate(byte[] certificateData)
        //    {
        //        return true;
        //    }
        //}
    }
}