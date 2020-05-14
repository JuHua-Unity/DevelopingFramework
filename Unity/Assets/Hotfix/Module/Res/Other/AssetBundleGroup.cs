using System.Collections.Generic;

namespace Hotfix
{
    public class AssetBundleGroup //开放给Editor使用
    {
        /// <summary>
        /// AB包属于哪个组
        /// 一个AB可能对应多个组(也就是说同时多个组都需要这个AB)
        /// </summary>
        public Dictionary<string, List<string>> Dic { get; set; }

        /// <summary>
        /// 获取某个组的AB
        /// </summary>
        /// <param name="group">组名</param>
        /// <returns></returns>
        public string[] GetABs(string group)
        {
            if (this.Dic == null)
            {
                return null;
            }

            return !this.Dic.TryGetValue(group, out var res) ? null : res.ToArray();
        }
    }
}