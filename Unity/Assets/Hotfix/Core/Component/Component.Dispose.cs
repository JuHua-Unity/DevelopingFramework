﻿namespace Hotfix
{
    internal partial class Component
    {
        public override void Dispose()
        {
            Warning($"Dispose->ID:{this.ObjId} Name:{GetType().FullName}");
            if (this.IsDisposed)
            {
                return;
            }

            OnceWaitTimerDispose();
            RepeatWaitTimerDispose();
            //先释放所有挂在自己身上的Component
            SingleSystemDispose();
            MultiSystemDispose();

            //释放自己
            Game.ComponentSystem.Destroy(this);
            Game.ComponentSystem.Remove(this.ObjId);
            this.Parent = null;

            //最后执行 防止在此之前还有需要判断IsDisposed
            base.Dispose();
        }
    }
}