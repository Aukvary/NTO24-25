using UnityEngine.Events;

namespace NTO24
{
    public interface IAttacker : IEntity
    {
        AttackController AttackController { get; }

        IHealthable Target
        {
            get => AttackController.Target;
            set
            {
                AttackController.Target = value;
            }
        }

        bool CanAttack => AttackController.CanAttack;

        float AttackRange => AttackController.Range;

        float Damage => AttackController.Damage;

        public void AddOnTargetChaneAction(UnityAction<IHealthable> action)
            => AttackController.AddOnTargetChaneAction(action);

        public void RemoveOnTargetChaneAction(UnityAction<IHealthable> action)
            => AttackController.RemoveOnTargetChaneAction(action);

        public void AddOnAttackAction(UnityAction<IHealthable> action)
            => AttackController.AddOnAttackAction(action);

        public void RemoveOnAttackAction(UnityAction<IHealthable> action)
            => AttackController.RemoveOnAttackAction(action);
    }
}