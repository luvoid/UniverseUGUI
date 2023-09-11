using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace UniverseLib.UI.Models
{
    public interface ISelectableModel<T> : IComponentModel<T>
        where T : Selectable
    {
    }
}
