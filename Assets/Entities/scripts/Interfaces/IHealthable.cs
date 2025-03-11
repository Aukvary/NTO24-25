using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public interface IHealthable : IEntity
    {
        EntityHealth HealthController { get; }

        float Health => HealthController.Health;
        float MaxHealth => HealthController.MaxHealth;
        float Regeneration => HealthController.Regeneration;
        bool Alive => HealthController.Alive;

        UnityEvent<Entity, HealthChangeType> OnHealthChangeEvent
            => HealthController.OnHealthChangeEvent;

        UnityEvent<Entity> OnDeathEvent => HealthController.OnDeathEvent;

        UnityEvent<bool> OnAliveChangeEvent
            => HealthController.OnAliveChangeEvent;

        UnityEvent OnUpgadeEvent
            => HealthController.OnUpgadeEvent;

        void Damage(float damage, Entity by = null)
        {
            if (damage < 0)
                throw new System.Exception("damage < 0");
            HealthController.ChangeHealth(damage, HealthChangeType.Damage, by);
        }

        void Heal(float heal, Entity by = null)
        {
            if (heal < 0)
                throw new System.Exception("heal < 0");
            HealthController.ChangeHealth(heal, HealthChangeType.Heal, by);
        }
    }
}