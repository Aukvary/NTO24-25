using System.Collections.Generic;

public interface IControllable : IEntity
{
    IUnitTask CurrentTask { get; }
    IEnumerable<IUnitTask> Tasks { get; }

    void SetTask(IUnitTask task)
    {

    }

    void AddTask(IUnitTask task)
    {

    }
}