using UnityEngine;
using UnityGameFramework.Runtime;

namespace Model
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField] private string resServerUrl = "www.baidu.com";

        public string ResServerUrl => this.resServerUrl;
    }
}