using System;
using System.Reflection;

namespace Editors
{
    internal interface IComponentViewDrawer
    {
        bool TypeEquals(Type type);
        object DrawAndGetNewValue(Type type, object value, DrawInfo draw, FieldInfo field);
    }
}