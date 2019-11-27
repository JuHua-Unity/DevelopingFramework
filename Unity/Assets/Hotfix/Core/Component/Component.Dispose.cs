namespace Hotfix
{
    internal partial class Component
    {
        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            //先释放所有挂在自己身上的Component
            SingleSystemDispose();
            MultiSystemDispose();

            //释放自己
            Game.ComponentSystem.Destroy(this);
            Game.ComponentSystem.Remove(ObjId);
            if (IsFromPool)
            {
                Parent = null;
                Game.ObjectPool.Recycle(this);
            }

#if UNITY_EDITOR && !ILRuntime && ComponentView

            else
            {
                UnityEngine.Object.Destroy(GameObject);
            }

#endif
        }
    }
}