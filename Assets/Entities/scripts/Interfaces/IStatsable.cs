using System.Collections.Generic;
using System.Linq;

public interface IStatsable : IEntity
{
    public IEnumerable<EntityStat> Stats { get; }

    public EntityStat this[EntityStatsType statsType]
    {
        get
        {
            var stat = Stats.First(s => s.Stat == statsType);

            return stat ?? throw new System.Exception($"Entity has no {statsType.ToString()}");
        }
    }

}