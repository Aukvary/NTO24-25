using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class Interactable : EntityComponent
    {
        [field: SerializeField]
        public UnityEvent<IInteractor> OnInteractEvent { get; private set; }

        public void Interact(IInteractor interactor)
            => OnInteractEvent.Invoke(interactor);
    }
}