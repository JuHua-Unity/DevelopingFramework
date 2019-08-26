using System;
using System.Collections.Generic;
using System.Reflection;

namespace Model
{
    /// <summary>
    /// 事件系统
    /// </summary>
    internal sealed class EventSystem : Object
    {
        private readonly Dictionary<long, Component> allComponents = new Dictionary<long, Component>();

        private readonly Dictionary<Type, List<Type>> types = new Dictionary<Type, List<Type>>();

        private readonly Dictionary<long, List<IEvent>> allEvents = new Dictionary<long, List<IEvent>>();

        private readonly Dictionary<Type, IAwakeSystem> awakeSystems = new Dictionary<Type, IAwakeSystem>();
        private readonly Dictionary<Type, IStartSystem> startSystems = new Dictionary<Type, IStartSystem>();
        private readonly Dictionary<Type, IDestroySystem> destroySystems = new Dictionary<Type, IDestroySystem>();
        private readonly Dictionary<Type, IUpdateSystem> updateSystems = new Dictionary<Type, IUpdateSystem>();
        private readonly Dictionary<Type, ILateUpdateSystem> lateUpdateSystems = new Dictionary<Type, ILateUpdateSystem>();

        private Queue<long> updates = new Queue<long>();
        private Queue<long> updates2 = new Queue<long>();
        private readonly Queue<long> starts = new Queue<long>();
        private Queue<long> lateUpdates = new Queue<long>();
        private Queue<long> lateUpdates2 = new Queue<long>();

        public void Init(Assembly assembly)
        {
            types.Clear();
            foreach (Type type in assembly.GetTypes())
            {
                object[] objects = type.GetCustomAttributes(typeof(BaseAttribute), false);
                if (objects.Length == 0)
                {
                    continue;
                }

                BaseAttribute baseAttribute = (BaseAttribute)objects[0];
                if (types.ContainsKey(baseAttribute.AttributeType))
                {
                    types[baseAttribute.AttributeType].Add(type);
                }
                else
                {
                    types.Add(baseAttribute.AttributeType, new List<Type>() { type });
                }
            }

            awakeSystems.Clear();
            lateUpdateSystems.Clear();
            updateSystems.Clear();
            startSystems.Clear();
            destroySystems.Clear();

            foreach (Type type in types[typeof(ComponentSystemAttribute)])
            {
                object[] attrs = type.GetCustomAttributes(typeof(ComponentSystemAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                object obj = Activator.CreateInstance(type);

                switch (obj)
                {
                    case IAwakeSystem awakeSystem:
                        awakeSystems.Add(awakeSystem.Type(), awakeSystem);
                        break;
                    case IUpdateSystem updateSystem:
                        updateSystems.Add(updateSystem.Type(), updateSystem);
                        break;
                    case ILateUpdateSystem lateUpdateSystem:
                        lateUpdateSystems.Add(lateUpdateSystem.Type(), lateUpdateSystem);
                        break;
                    case IStartSystem startSystem:
                        startSystems.Add(startSystem.Type(), startSystem);
                        break;
                    case IDestroySystem destroySystem:
                        destroySystems.Add(destroySystem.Type(), destroySystem);
                        break;
                }
            }

            allEvents.Clear();
            foreach (Type type in types[typeof(EventAttribute)])
            {
                object[] attrs = type.GetCustomAttributes(typeof(EventAttribute), false);
                foreach (object attr in attrs)
                {
                    EventAttribute aEventAttribute = (EventAttribute)attr;
                    object obj = Activator.CreateInstance(type);
                    IEvent iEvent = obj as IEvent;
                    if (iEvent == null)
                    {
                        Log.Error($"{obj.GetType().Name} 没有继承IEvent");
                    }
                    RegisterEvent(aEventAttribute.ID, iEvent);
                }
            }
        }

        public void RegisterEvent(long eventId, IEvent e)
        {
            if (!allEvents.ContainsKey(eventId))
            {
                allEvents.Add(eventId, new List<IEvent>());
            }
            allEvents[eventId].Add(e);
        }

        public List<Type> GetTypes(Type systemAttributeType)
        {
            if (!types.ContainsKey(systemAttributeType))
            {
                return new List<Type>();
            }
            return types[systemAttributeType];
        }

        public void Add(Component component)
        {
            allComponents.Add(component.ObjId, component);

            Type type = component.GetType();

            if (updateSystems.ContainsKey(type))
            {
                updates.Enqueue(component.ObjId);
            }

            if (startSystems.ContainsKey(type))
            {
                starts.Enqueue(component.ObjId);
            }

            if (lateUpdateSystems.ContainsKey(type))
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
            if (!awakeSystems.TryGetValue(component.GetType(), out IAwakeSystem iAwakeSystem))
            {
                return;
            }

            if (iAwakeSystem == null)
            {
                return;
            }

            if (!(iAwakeSystem is IAwake iAwake))
            {
                return;
            }

#if UNITY_EDITOR
            iAwake.Run(component);
#else
            try
            {
                iAwake.Run(component);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
#endif
        }

        public void Awake<P1>(Component component, P1 p1)
        {
            if (!awakeSystems.TryGetValue(component.GetType(), out IAwakeSystem iAwakeSystem))
            {
                return;
            }

            if (iAwakeSystem == null)
            {
                return;
            }

            if (!(iAwakeSystem is IAwake<P1> iAwake))
            {
                return;
            }

#if UNITY_EDITOR
            iAwake.Run(component, p1);
#else
            try
            {
                iAwake.Run(component, p1);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
#endif
        }

        public void Awake<P1, P2>(Component component, P1 p1, P2 p2)
        {
            if (!awakeSystems.TryGetValue(component.GetType(), out IAwakeSystem iAwakeSystem))
            {
                return;
            }

            if (iAwakeSystem == null)
            {
                return;
            }

            if (!(iAwakeSystem is IAwake<P1, P2> iAwake))
            {
                return;
            }

#if UNITY_EDITOR
            iAwake.Run(component, p1, p2);
#else
            try
            {
                iAwake.Run(component, p1, p2);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
#endif
        }

        public void Awake<P1, P2, P3>(Component component, P1 p1, P2 p2, P3 p3)
        {
            if (!awakeSystems.TryGetValue(component.GetType(), out IAwakeSystem iAwakeSystem))
            {
                return;
            }

            if (iAwakeSystem == null)
            {
                return;
            }

            if (!(iAwakeSystem is IAwake<P1, P2, P3> iAwake))
            {
                return;
            }

#if UNITY_EDITOR
            iAwake.Run(component, p1, p2, p3);
#else
            try
            {
                iAwake.Run(component, p1, p2, p3);
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

                if (!startSystems.TryGetValue(component.GetType(), out IStartSystem iStartSystem))
                {
                    continue;
                }

#if UNITY_EDITOR
                iStartSystem.Run(component);
#else
                try
                {
                    iStartSystem.Run(component);
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
            if (!destroySystems.TryGetValue(component.GetType(), out IDestroySystem iDestroySystem))
            {
                return;
            }

            if (iDestroySystem == null)
            {
                return;
            }

#if UNITY_EDITOR
            iDestroySystem.Run(component);
#else
            try
            {
                iDestroySystem.Run(component);
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

                if (updateSystems.TryGetValue(component.GetType(), out IUpdateSystem iUpdateSystem))
                {
                    continue;
                }

                updates2.Enqueue(ObjId);

#if UNITY_EDITOR
                iUpdateSystem.Run(component);
#else
                try
                {
                    iUpdateSystem.Run(component);
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

                if (lateUpdateSystems.TryGetValue(component.GetType(), out ILateUpdateSystem iLateUpdateSystem))
                {
                    continue;
                }

                lateUpdates2.Enqueue(ObjId);

#if UNITY_EDITOR
                iLateUpdateSystem.Run(component);
#else
                try
                {
                    iLateUpdateSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
#endif
            }

            Swap(ref lateUpdates, ref lateUpdates2);
        }

        public void Run<A>(long id, A a)
        {
            if (!allEvents.TryGetValue(id, out List<IEvent> iEvents))
            {
                return;
            }
            foreach (IEvent iEvent in iEvents)
            {
                try
                {
                    iEvent?.Handle(a);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Run<A, B>(long id, A a, B b)
        {
            if (!allEvents.TryGetValue(id, out List<IEvent> iEvents))
            {
                return;
            }
            foreach (IEvent iEvent in iEvents)
            {
                try
                {
                    iEvent?.Handle(a, b);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Run<A, B, C>(long id, A a, B b, C c)
        {
            if (!allEvents.TryGetValue(id, out List<IEvent> iEvents))
            {
                return;
            }
            foreach (IEvent iEvent in iEvents)
            {
                try
                {
                    iEvent?.Handle(a, b, c);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
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
    }
}