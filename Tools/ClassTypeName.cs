using System;
using UnityEngine;

namespace Series.Core
{
    public class ClassTypeName : PropertyAttribute
    {
        public Type type;

        public ClassTypeName(Type type)
        {
            this.type = type;
        }
    }
}
