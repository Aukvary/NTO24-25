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

        void AddOnHealthChangeAction(UnityAction<Entity, HealthChangeType> action)
            => HealthController.AddOnHealthChangeAction(action);
        void RemoveOnHealthChangeAction(UnityAction<Entity, HealthChangeType> action)
            => HealthController.RemoveOnHealthChangeAction(action);

        void AddOnDeathAction(UnityAction<Entity> action)
            => HealthController.AddOnDeathAction(action);
        void RemoveOnDeathAction(UnityAction<Entity> action)
            => HealthController.RemoveOnDeathAction(action);

        void AddOnAliveChangeAction(UnityAction<bool> action)
            => HealthController.AddOnAliveChangeAction(action);
        void RemoveOnAliveChangeAction(UnityAction<bool> action)
            => HealthController.RemoveOnAliveChangeAction(action);

        void AddOnUpgradeAction(UnityAction action)
            => HealthController.AddOnUpgradeAction(action);
        void RemoveOnUpgradeAction(UnityAction action)
            => HealthController.RemoveOnUpgradeAction(action);
    }
}