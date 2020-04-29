using System.Collections.Generic;
using Async;

namespace Hotfix
{
    internal class CancellationTokenSource : Component, IAwakeSystem<long>, IDestroySystem
    {
        private List<CancellationToken> cancellationTokens;

        public void Awake(long a)
        {
            CancelAfter(a).Coroutine();
        }

        public void Destroy()
        {
            this.cancellationTokens?.Clear();
        }

        public void Cancel()
        {
            if (this.cancellationTokens == null)
            {
                return;
            }

            foreach (var token in this.cancellationTokens)
            {
                token.Cancel();
            }
        }

        /// <summary>
        /// 注意：此操作是不可取消的
        /// </summary>
        /// <param name="afterTimeCancel">毫秒</param>
        /// <returns></returns>
        public async Void CancelAfter(long afterTimeCancel)
        {
            if (this.cancellationTokens == null)
            {
                return;
            }

            var objId = this.ObjId;
            await TimerComponent.Instance.WaitAsync(afterTimeCancel);
            if (this.ObjId != objId)
            {
                return;
            }

            Cancel();
        }

        /// <summary>
        /// 每次都会生成新的Token
        /// 如果你需要进行多次Token.Register 那么你需要自己保存这个Token实例
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                var cancellationToken = AddMultiComponent<CancellationToken>();
                Add(cancellationToken);
                return cancellationToken;
            }
        }

        private void Add(CancellationToken cancellationToken)
        {
            if (this.cancellationTokens == null)
            {
                this.cancellationTokens = new List<CancellationToken>();
            }

            this.cancellationTokens.Add(cancellationToken);
        }
    }
}