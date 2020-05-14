using System;
using System.Reflection;

namespace ObjectDrawer
{
    internal interface IObjectDrawer
    {
        int Priority { get; }
        bool TypeEquals(Type t);
        void Draw(object value, FieldInfo field = null);
    }
}