using UnityEngine;

namespace Model
{
    internal sealed class GameStart
    {
        /// <summary>
        /// Model层游戏启动流程
        /// </summary>
        /// <returns></returns>
        public async Void GameStartAsync(TextAsset text)
        {
            LaunchOptions options = LitJson.JsonMapper.ToObject<LaunchOptions>(text.text);
            await Game.Resources.InitDownload(options);
            await Game.Resources.InitResources();
        }
    }
}