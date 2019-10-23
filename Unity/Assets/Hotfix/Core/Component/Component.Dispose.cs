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
            foreach (var item in components)
            {
#if UNITY_EDITOR
                item.Value.Dispose();
#else
                try
                {
                    item.Value.Dispose();
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
#endif
            }

            components.Clear();

            //释放自己
            Game.ComponentSystem.Destroy(this);
            Game.ComponentSystem.Remove(ObjId);
            if (IsFromPool)
            {
                ComponentFactory.Delete(this);
            }
#if UNITY_EDITOR
            else
            {
                UnityEngine.Object.Destroy(GameObject);
            }
#endif
        }
    }
}