using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class Interactable : EntityComponent
    {
        [SerializeField]
        private UnityEvent<IInteractor> _onInteractEvent;

        public void Interact(IInteractor interactor)
        {
            _onInteractEvent.Invoke(interactor);
        }

        public void AddOnInteractAction(UnityAction<IInteractor> action)
            => _onInteractEvent.AddListener(action);

        public void RemoveOnInteractAction(UnityAction<IInteractor> action)
            => _onInteractEvent.RemoveListener(action);
    }
}