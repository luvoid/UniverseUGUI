using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace UniverseLib.UGUI.ImplicitTypes
{
    public interface IReadOnlyObservable<T>
    {
        public T Value { get; }

        /// <summary>
        /// Invoked when <see cref="Value"/> is changed via user input.
        /// </summary>
        /// <remarks>
        /// It is not invoked when <see cref="IObservable{T}.Value"/> is set.
        /// </remarks>
        public event UnityAction<T> Changed;
    }
    public interface IObservable<T> : IReadOnlyObservable<T>
    {
        public new T Value { get; set; }
    }
}
