using System.Collections.Generic;
using System.Linq;

public interface IStatsable : IEntity
{
    public IEnumerable<EntityStat> Stats { get; }

    public EntityStat this[EntityStatsType statsType] { get; }

}