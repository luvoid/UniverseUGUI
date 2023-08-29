using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniverseLib.UI;

namespace UniverseLib.UGUI.ImplicitTypes
{
    internal class ReadOnlyRef<T>
    {
        protected readonly Getter<T> getter;
        public T Value => getter.Invoke();
        public bool IsConstant { get; protected set; }

        public ReadOnlyRef(T value)
        {
            getter = () => value;
            IsConstant = true;
        }

        public ReadOnlyRef(Getter<T> getter)
        {
            this.getter = getter;
            IsConstant = false;
        }

        public static explicit operator ReadOnlyRef<T>(T value)
        {
            return new ReadOnlyRef<T>(value);
        }

        public static implicit operator T(ReadOnlyRef<T> readOnlyRef)
        {
            return readOnlyRef.Value;
        }

        public static implicit operator ReadOnlyRef<T>(Getter<T> getter)
        {
            return new ReadOnlyRef<T>(getter);
        }
    }
}
