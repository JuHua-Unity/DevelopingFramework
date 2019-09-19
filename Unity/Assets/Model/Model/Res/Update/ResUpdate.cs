using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace Model
{
    internal sealed class ResUpdate
    {
        private UnityWebRequestAsync request = new UnityWebRequestAsync();

        private bool needUpdate = false;
        private string resServerUrl;
        private ResControl resControl;
        private ResVersion localResVersion;
        private ResVersion remoteResVersion;

        private long totalSize = 0;
        private List<string> downloaded = new List<string>();

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
                Clear();
                return true;
            }

            if (!await Check())
            {
                Clear();
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
                    Clear();
                    return true;
                }
            }

            Clear();
            return false;
        }

        public float Progress
        {
            get
            {
                if (totalSize == 0)
                {
                    return 1;
                }

                long t = (long)request.Bytes;
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

                return t * 1f / totalSize;
            }
        }

        private void Clear()
        {
            downloaded.Clear();
            totalSize = 0;
        }

        private async Task<bool> Download(List<string> download)
        {
            for (int i = 0; i < download.Count; i++)
            {
                try
                {
                    string file = download[i];
                    Log.Debug($"开始下载：{file}");
                    DownloadHandler res = await request.DownloadAsync($"{resServerUrl}{file}");
                    byte[] data = res.data;
                    string path = Path.Combine(PathHelper.AppHotfixResPath, file);
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }

                    downloaded.Add(file);
                    ClearRequest();
                    Log.Debug($"{file}下载完成！");
                }
                catch (System.Exception e)
                {
                    Log.Exception(e);
                    ClearRequest();
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
            string url = $"{PathHelper.AppResPath4Web}ResVersion.json";
            Log.Debug($"本地资源版本URL:{url}");
            try
            {
                localResVersion = LitJson.JsonMapper.ToObject<ResVersion>((await request.DownloadAsync(url)).text);
                ClearRequest();
            }
            catch (System.Exception e)
            {
                Log.Exception(e);
                ClearRequest();
                return false;
            }

            return true;
        }

        private async Task<bool> LoadRemoteResVersion()
        {
            string url = $"{resServerUrl}ResVersion.json";
            Log.Debug($"服务器资源版本URL:{url}");
            try
            {
                remoteResVersion = LitJson.JsonMapper.ToObject<ResVersion>((await request.DownloadAsync(url)).text);
                ClearRequest();
            }
            catch (System.Exception e)
            {
                Log.Exception(e);
                ClearRequest();
                return false;
            }

            return true;
        }

        private void ClearRequest()
        {
            request.Dispose();
        }
    }
}