using UnityEngine.Events;

public interface IAttacker : IEntity
{
    IHealthable Target { get; }

    UnityEvent<IHealthable> OnSetTargetEvent { get; }

    void Attack(IHealthable target)
    {
        OnSetTargetEvent.Invoke(target);
    }
}