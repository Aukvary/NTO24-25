using UnityEngine.Events;

namespace NTO24
{
    public interface IAttacker : IEntity
    {
        AttackController AttackController { get; }

        IHealthable Target
        {
            get => AttackController.Target;
            set => AttackController.Target = value;
        }

        UnityEvent<IHealthable> OnChangeTargetEvent 
            => AttackController.OnChangeTargetEvent;

        UnityEvent<IHealthable> OnAttackEvent
            => AttackController.OnAttackEvent;

        bool CanAttack => AttackController.CanAttack;

        float AttackRange => AttackController.Range;

        float Damage => AttackController.Damage;
    }
}