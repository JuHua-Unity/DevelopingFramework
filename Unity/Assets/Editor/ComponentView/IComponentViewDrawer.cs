using System;
using System.Reflection;

namespace Editors
{
    internal interface IComponentViewDrawer
    {
        bool TypeEquals(Type type);
        object DrawAndGetNewValue(Type type, string name, object value, bool changeable, bool staticField, FieldInfo field);
    }
}