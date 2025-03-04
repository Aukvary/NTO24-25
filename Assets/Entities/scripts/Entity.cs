using UnityEngine;

namespace NTO24
{
    public abstract class Entity : MonoBehaviour
    {
        [field: SerializeField]
        public EntityType EntityType { get; private set; }

        public Entity EntityReference => this;

        protected virtual void Awake() { }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }
    }
}