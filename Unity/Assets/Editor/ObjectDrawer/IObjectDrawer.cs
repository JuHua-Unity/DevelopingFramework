using System;
using System.Reflection;

namespace Editors
{
    internal interface IObjectDrawer
    {
        int Priority { get; }
        bool TypeEquals(Type type);
        object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field);
    }
}