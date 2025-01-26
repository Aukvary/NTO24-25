public interface IHealthable : IEntity
{
    public EntityHealth HealthComponent { get; }

    public float Health => HealthComponent.Health;
    public float Regeneration => HealthComponent.Regeneration;
    public bool Alive => HealthComponent.Alive;

    public void Damage(float damage, Entity by = null)
    {
        HealthComponent.ChangeHealth(damage, HealthChangeType.Damage);
    }

    public void Heal(float heal, Entity by = null)
    {
        HealthComponent.ChangeHealth(heal, HealthChangeType.Heal);
    }
}