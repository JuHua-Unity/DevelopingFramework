using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal class CancellationToken : Component, IDestroySystem
    {
        private readonly List<Action> actions = new List<Action>();

        public void Register(Action callback)
        {
            if (callback == null)
            {
                return;
            }

            if (!this.actions.Contains(callback))
            {
                this.actions.Add(callback);
            }
        }

        public void Cancel()
        {
            var a = this.actions.ToArray();
            for (var i = 0; i < a.Length; i++)
            {
                a[i]?.Invoke();
            }
        }

        public void Destroy()
        {
            this.actions.Clear();
        }
    }
}