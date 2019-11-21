﻿using System;
using System.Reflection;

namespace Editors
{
    internal interface IObjectDrawer
    {
        int Priority { get; }
        bool TypeEquals(Type type);
        void Draw(object value, FieldInfo field = null);
    }
}