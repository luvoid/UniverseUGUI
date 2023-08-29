using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace UniverseLib.UGUI.ImplicitTypes
{
    internal static class Observable
    {
        private class DefaultComparer<T> : IEqualityComparer<T>
            where T : IEquatable<T>
        {
            public bool Equals(T x, T y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        public static Observable<T> Create<T>(T value = default)
            where T : IEquatable<T>
        {
            return new Observable<T>(new DefaultComparer<T>(), value);
        }
    }

    internal class Observable<T> : IObservable<T>
    {

        private IEqualityComparer<T> comparer;
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (!comparer.Equals(_value, value))
                {
                    _value = value;
                    ChangedInternal.Invoke(value);
                }
            }
        }
        public event UnityAction<T> Changed;
        public event UnityAction<T> ChangedInternal;

        internal Observable(IEqualityComparer<T> comparer, T value = default)
        {
            _value = value;
            this.comparer = comparer;
        }

        internal void SetFromUI(T value)
        {
            if (!comparer.Equals(_value, value))
            {
                _value = value;
                Changed.Invoke(value);
            }
        }
    }
}
