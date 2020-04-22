namespace Hotfix
{
    internal partial class Component
    {
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            //先释放所有挂在自己身上的Component
            SingleSystemDispose();
            MultiSystemDispose();

            //释放自己
            Game.ComponentSystem.Destroy(this);
            Game.ComponentSystem.Remove(this.ObjId);
            if (this.IsFromPool)
            {
                this.Parent = null;
                Game.ObjectPool.Recycle(this);
            }
#if UNITY_EDITOR && !ILRuntime && ComponentView

            else
            {
                UnityEngine.Object.DestroyImmediate(this.GameObject);
            }

#endif
        }
    }
}