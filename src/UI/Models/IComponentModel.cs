using UnityEngine.EventSystems;

namespace UniverseLib.UI.Models
{
    public interface IComponentModel<T>
        where T : UIBehaviour
    {
        public T Component { get; }
    }
}
