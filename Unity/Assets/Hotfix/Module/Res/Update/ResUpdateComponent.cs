using Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace Hotfix
{
    internal class ResUpdateComponent : Component, IDestroySystem
    {
        private bool needUpdate = false;
        private string resServerUrl;

        private ResVersion localResVersion;
        private ResVersion remoteResVersion;

        private UnityWebRequestAsync request;
        private long totalSize = 0;
        private readonly List<string> downloaded = new List<string>();

        public void SetUrl(string url)
        {
            resServerUrl = url;
        }

        public void SetNeedUpdate(bool u)
        {
            needUpdate = u;//判断是否需要更新 (注意：这里仅仅是游戏更新前的初始判断 比如游戏在审核 不能进行更新  或者 游戏肯定不会有更新)
        }

        public async Task<bool> UpdateRes(string group)
        {
            if (request.Downloading)
            {
                return false;
            }

            if (!needUpdate)
            {
                return true;
            }

            if (!await Check())
            {
                return false;
            }

            if (remoteResVersion.Res.TryGetValue(group, out List<ResVersion.ResVersionInfo> list))
            {
                List<string> download = new List<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!download.Contains(list[i].File) && !downloaded.Contains(list[i].File))
                    {
                        if (!GetBundleMD5(group, list[i].File).Equals(list[i].MD5))
                        {
                            download.Add(list[i].File);
                            totalSize += list[i].Size;
                        }
                    }
                }

                if (await Download(download))
                {
                    return true;
                }
            }

            return false;
        }

        public long Bytes
        {
            get
            {
                if (totalSize == 0)
                {
                    return 0;
                }

                long t = 0;
                if (request != null)
                {
                    t += (long)request.Bytes;
                }

                if (remoteResVersion != null)
                {
                    for (int i = 0; i < downloaded.Count; i++)
                    {
                        foreach (var item in remoteResVersion.Res)
                        {
                            for (int j = 0; j < item.Value.Count; j++)
                            {
                                if (item.Value[j].File.Equals(downloaded[i]))
                                {
                                    t += item.Value[j].Size;
                                }
                            }
                        }
                    }
                }

                return t;
            }
        }

        public float Progress
        {
            get
            {
                if (totalSize == 0)
                {
                    return 1;
                }

                return Bytes * 1f / totalSize;
            }
        }

        private async Task<bool> Download(List<string> download)
        {
            for (int i = 0; i < download.Count; i++)
            {
                try
                {
                    string file = download[i];
                    Log.Debug($"开始下载：{file}");
                    CreateRequest();
                    DownloadHandler res = await request.DownloadAsync($"{resServerUrl}{file}");
                    byte[] data = res.data;
                    string path = Path.Combine(PathHelper.AppHotfixResPath, file);
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }

                    CloseRequest();
                    downloaded.Add(file);
                    Log.Debug($"{file}下载完成！");
                }
                catch (System.Exception e)
                {
                    Log.Exception(e);
                    return false;
                }
            }

            return true;
        }

        private string GetBundleMD5(string group, string file)
        {
            string path = Path.Combine(PathHelper.AppHotfixResPath, file);
            if (File.Exists(path))
            {
                return MD5Helper.FileMD5(path);
            }

            if (localResVersion.Res.TryGetValue(group, out List<ResVersion.ResVersionInfo> infos))
            {
                for (int i = 0; i < infos.Count; i++)
                {
                    if (infos[i].File.Equals(file))
                    {
                        return infos[i].MD5;
                    }
                }
            }

            return "";
        }

        private async Task<bool> Check()
        {
            if (string.IsNullOrEmpty(resServerUrl))
            {
                return false;
            }

            if (localResVersion == null)
            {
                if (!await LoadLocalResVersion())
                {
                    return false;
                }
            }

            if (remoteResVersion == null)
            {
                if (!await LoadRemoteResVersion())
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> LoadLocalResVersion()
        {
            bool result = true;
            string url = $"{PathHelper.AppResPathForWeb}ResVersion.json";
            Log.Debug($"本地资源版本URL:{url}");
            CreateRequest();
            try
            {
                localResVersion = JsonHelper.FromJson<ResVersion>((await request.DownloadAsync(url)).text);
            }
            catch (System.Exception e)
            {
                Log.Exception(e);
                result = false;
            }

            CloseRequest();
            return result;
        }

        private async Task<bool> LoadRemoteResVersion()
        {
            bool result = true;
            string url = $"{resServerUrl}ResVersion.json";
            Log.Debug($"服务器资源版本URL:{url}");
            CreateRequest();
            try
            {
                remoteResVersion = JsonHelper.FromJson<ResVersion>((await request.DownloadAsync(url)).text);
            }
            catch (System.Exception e)
            {
                Log.Exception(e);
                result = false;
            }

            CloseRequest();
            return result;
        }

        private void CreateRequest()
        {
            request = ComponentFactory.Create<UnityWebRequestAsync>(this);
        }

        private void CloseRequest()
        {
            RemoveComponent<UnityWebRequestAsync>();
            request = null;
        }

        public void Destroy()
        {
            totalSize = 0;
            downloaded.Clear();
            if (request != null)
            {
                CloseRequest();
            }
        }
    }
}