using System;
using System.Collections.Generic;

namespace Hotfix
{
    internal sealed class ComponentSystem : Object
    {
        private readonly Dictionary<long, Component> allComponents = new Dictionary<long, Component>();

        private Queue<long> updates = new Queue<long>();
        private Queue<long> updates2 = new Queue<long>();
        private readonly Queue<long> starts = new Queue<long>();
        private Queue<long> lateUpdates = new Queue<long>();
        private Queue<long> lateUpdates2 = new Queue<long>();

        public void Add(Component component)
        {
            if (component.IsDisposed)
            {
                return;
            }

            allComponents.Add(component.ObjId, component);

            Type type = component.GetType();

            if (type is IUpdateSystem)
            {
                updates.Enqueue(component.ObjId);
            }

            if (type is IStartSystem)
            {
                starts.Enqueue(component.ObjId);
            }

            if (type is ILateUpdateSystem)
            {
                lateUpdates.Enqueue(component.ObjId);
            }
        }

        public void Remove(long ObjId)
        {
            allComponents.Remove(ObjId);
        }

        public void Awake(Component component)
        {
            if (component.IsDisposed)
            {
                return;
            }

            if (!allComponents.ContainsKey(component.ObjId))
            {
                return;
            }

            if (!(component is IAwakeSystem awakeSystem))
            {
                return;
            }

#if UNITY_EDITOR
            awakeSystem.Awake();
#else
            try
            {
                awakeSystem.Awake();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
#endif
        }

        public void Awake<P1>(Component component, P1 p1)
        {
            if (component.IsDisposed)
            {
                return;
            }

            if (!allComponents.ContainsKey(component.ObjId))
            {
                return;
            }

            if (!(component is IAwakeSystem<P1> awakeSystem))
            {
                return;
            }

#if UNITY_EDITOR
            awakeSystem.Awake(p1);
#else
            try
            {
                awakeSystem.Awake(p1);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
#endif
        }

        public void Awake<P1, P2>(Component component, P1 p1, P2 p2)
        {
            if (component.IsDisposed)
            {
                return;
            }

            if (!allComponents.ContainsKey(component.ObjId))
            {
                return;
            }

            if (!(component is IAwakeSystem<P1, P2> awakeSystem))
            {
                return;
            }

#if UNITY_EDITOR
            awakeSystem.Awake(p1, p2);
#else
            try
            {
                awakeSystem.Awake(p1, p2);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
#endif
        }

        public void Awake<P1, P2, P3>(Component component, P1 p1, P2 p2, P3 p3)
        {
            if (component.IsDisposed)
            {
                return;
            }

            if (!allComponents.ContainsKey(component.ObjId))
            {
                return;
            }

            if (!(component is IAwakeSystem<P1, P2, P3> awakeSystem))
            {
                return;
            }

#if UNITY_EDITOR
            awakeSystem.Awake(p1, p2, p3);
#else
            try
            {
                awakeSystem.Awake(p1, p2, p3);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
#endif
        }

        private void Start()
        {
            while (starts.Count > 0)
            {
                long ObjId = starts.Dequeue();
                if (!allComponents.TryGetValue(ObjId, out Component component))
                {
                    continue;
                }

                if (!(component is IStartSystem startSystem))
                {
                    continue;
                }

#if UNITY_EDITOR
                startSystem.Start();
#else
                try
                {
                    startSystem.Start();
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
#endif
            }
        }

        public void Destroy(Component component)
        {
            if (component.IsDisposed)
            {
                return;
            }

            if (!allComponents.ContainsKey(component.ObjId))
            {
                return;
            }

            if (!(component is IDestroySystem destroySystem))
            {
                return;
            }

#if UNITY_EDITOR
            destroySystem.Destroy();
#else
            try
            {
                destroySystem.Destroy();
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
#endif
        }

        public void Update()
        {
            Start();

            while (updates.Count > 0)
            {
                long ObjId = updates.Dequeue();
                if (!allComponents.TryGetValue(ObjId, out Component component))
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                if (!(component is IUpdateSystem updateSystem))
                {
                    continue;
                }

                updates2.Enqueue(ObjId);

#if UNITY_EDITOR
                updateSystem.Update();
#else
                try
                {
                    updateSystem.Update();
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
#endif
            }

            Swap(ref updates, ref updates2);
        }

        public void LateUpdate()
        {
            while (lateUpdates.Count > 0)
            {
                long ObjId = lateUpdates.Dequeue();
                if (!allComponents.TryGetValue(ObjId, out Component component))
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                if (!(component is ILateUpdateSystem lateUpdateSystem))
                {
                    continue;
                }

                lateUpdates2.Enqueue(ObjId);

#if UNITY_EDITOR
                lateUpdateSystem.LateUpdate();
#else
                try
                {
                    lateUpdateSystem.LateUpdate();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
#endif
            }

            Swap(ref lateUpdates, ref lateUpdates2);
        }

        #region 交换两个update队列

        private static Queue<long> t3;

        /// <summary>
        /// 交换两个update队列
        /// </summary>
        /// <param name = "t1" ></ param >
        /// < param name="t2"></param>
        private static void Swap(ref Queue<long> t1, ref Queue<long> t2)
        {
            t3 = t1;
            t1 = t2;
            t2 = t3;
        }

        #endregion

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            allComponents.Clear();
            updates.Clear();
            updates2.Clear();
            starts.Clear();
            lateUpdates.Clear();
            lateUpdates2.Clear();
        }
    }
}