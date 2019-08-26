using System;

namespace Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class EventAttribute : BaseAttribute
    {
        public long ID { get; }

        public EventAttribute(long id)
        {
            ID = id;
        }
    }
}