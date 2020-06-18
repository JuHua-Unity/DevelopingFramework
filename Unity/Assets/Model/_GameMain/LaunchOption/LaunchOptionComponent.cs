using UnityEngine;
using UnityGameFramework.Runtime;

namespace Model
{
    public class LaunchOptionComponent : GameFrameworkComponent
    {
        [Header("资源服务器：")] [SerializeField] private string resServerUrl = "www.baidu.com";

        public string ResServerUrl => this.resServerUrl;
    }
}