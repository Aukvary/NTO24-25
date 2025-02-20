public interface IHealthable : IEntity
{
    public EntityHealth HealthComponent { get; }

    public float Health => HealthComponent.Health;
    public float Regeneration => HealthComponent.Regeneration;
    public bool Alive => HealthComponent.Alive;

    public void Damage(float damage, Entity by = null)
    {
        if (damage < 0)
            throw new System.Exception("damage < 0");
        HealthComponent.ChangeHealth(damage, HealthChangeType.Damage);
    }

    public void Heal(float heal, Entity by = null)
    {
        if (heal < 0)
            throw new System.Exception("heal < 0");
        HealthComponent.ChangeHealth(heal, HealthChangeType.Heal);
    }
}