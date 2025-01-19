public interface IInteractor : IEntity
{
    InteractingBehaviour InteractorComponent { get; }

    new void Initialize()
    {
        if (EntityReference is not IStatsable stats)
            throw new System.Exception("stats component was missed");
    }
}