using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniverseLib.UI.Styles
{
    public interface IDeepCopyable<T> where T : IDeepCopyable<T>
    {
        /// <summary>
        /// Returns a deep copy of the <typeparamref name="T"/>.
        /// </summary>
        public T DeepCopy();
    }
}
