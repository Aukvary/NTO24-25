using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class EntityTrigger : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<Entity> OnEntityEnter { get; private set; }

        [field: SerializeField]
        public UnityEvent<Entity> OnEntityExit { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Entity>(out var entity))
                OnEntityEnter.Invoke(entity);

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Entity>(out var entity))
                OnEntityExit.Invoke(entity);
        }
    }
}