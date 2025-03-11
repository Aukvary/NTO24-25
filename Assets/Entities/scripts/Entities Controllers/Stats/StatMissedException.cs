namespace NTO24
{
    public class StatMissedException : System.Exception
    {
        private IStatsable _entity;
        private StatNames _statName;

        public override string Message => $"enity({_entity.EntityReference.name}) has no {_statName}";

        public StatMissedException(IStatsable entity, StatNames statName)
        {
            _entity = entity;
            _statName = statName;
        }
    }
}