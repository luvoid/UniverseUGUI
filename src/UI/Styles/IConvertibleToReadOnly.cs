using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniverseLib.UI.Styles
{
    public interface IConvertibleToReadOnly<TReadOnly>
    {
        public TReadOnly AsReadOnly();
    }
}
