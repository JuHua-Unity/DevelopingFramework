using System.Collections.Generic;

namespace Hotfix
{
    internal sealed class ComponentSystem : Object
    {
        private readonly Dictionary<ulong, Component> components = new Dictionary<ulong, Component>();

        private Queue<ulong> updates = new Queue<ulong>();
        private Queue<ulong> updates2 = new Queue<ulong>();
        private readonly Queue<ulong> starts = new Queue<ulong>();
        private Queue<ulong> lateUpdates = new Queue<ulong>();
        private Queue<ulong> lateUpdates2 = new Queue<ulong>();

        public void Add(Component component)
        {
            if (component.IsDisposed)
            {
                return;
            }

            var id = component.ObjId;
            this.components.Add(id, component);

            switch (component)
            {
                case IUpdateSystem _:
                    this.updates.Enqueue(id);
                    break;
                case IStartSystem _:
                    this.starts.Enqueue(id);
                    break;
                case ILateUpdateSystem _:
                    this.lateUpdates.Enqueue(id);
                    break;
            }
        }

        public void Remove(ulong id)
        {
            this.components.Remove(id);
        }

        public void Awake(Component component)
        {
            if (component.IsDisposed)
            {
                return;
            }

            if (!this.components.ContainsKey(component.ObjId))
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

            if (!this.components.ContainsKey(component.ObjId))
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

            if (!this.components.ContainsKey(component.ObjId))
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

            if (!this.components.ContainsKey(component.ObjId))
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
            while (this.starts.Count > 0)
            {
                var id = this.starts.Dequeue();
                if (!this.components.TryGetValue(id, out var component))
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

            if (!this.components.ContainsKey(component.ObjId))
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

            while (this.updates.Count > 0)
            {
                var id = this.updates.Dequeue();
                if (!this.components.TryGetValue(id, out var component))
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

                this.updates2.Enqueue(id);

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

            Swap(ref this.updates, ref this.updates2);
        }

        public void LateUpdate()
        {
            while (this.lateUpdates.Count > 0)
            {
                var id = this.lateUpdates.Dequeue();
                if (!this.components.TryGetValue(id, out var component))
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

                this.lateUpdates2.Enqueue(id);

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

            Swap(ref this.lateUpdates, ref this.lateUpdates2);
        }

        #region 交换两个update队列

        private static Queue<ulong> t3;

        /// <summary>
        /// 交换两个update队列
        /// </summary>
        /// <param name = "t1" ></param >
        /// < param name="t2"></param>
        private static void Swap(ref Queue<ulong> t1, ref Queue<ulong> t2)
        {
            t3 = t1;
            t1 = t2;
            t2 = t3;
        }

        #endregion

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.components.Clear();
            this.updates.Clear();
            this.updates2.Clear();
            this.starts.Clear();
            this.lateUpdates.Clear();
            this.lateUpdates2.Clear();
        }
    }
}