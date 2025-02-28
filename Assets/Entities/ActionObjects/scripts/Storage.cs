using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace NTO24
{
    public class Storage : Entity, IInteractable, ILoadable
    {
        [field: SerializeField]
        public UnityEvent<IInteractor> OnInteracEvent { get; private set; }

        public bool Interactable => true;

        public int CellCount => Resources.ResourceNames.Count();
    }
}