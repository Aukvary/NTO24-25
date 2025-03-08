using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    public abstract class Entity : MonoBehaviour
    {
        [field: SerializeField]
        public EntityType EntityType { get; private set; }

        public Entity EntityReference => this;

        private static List<Entity> _entities;

        static Entity()
        {
            _entities = new(100);
        }

        public static IEnumerable<Entity> Entities
            => _entities;

        public static IEnumerable<T> GetEntites<T>()
            => _entities.Where(e => e is T).Cast<T>();

        public static void Add(Entity entity)
        {
            _entities.Add(entity);

            if (entity is IHealthable health && entity is not IRestoreable)
                health.OnDeathEvent.AddListener(e => _entities.Remove(entity));
        }

        public static void Clear()
            => _entities.Clear();

        protected virtual void Awake()
        {
            Add(this);
        }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }


    }
}