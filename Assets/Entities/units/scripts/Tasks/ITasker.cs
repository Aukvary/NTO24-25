using System.Collections.Generic;

public interface ITasker : IEntity
{
    IEnumerable<UnitTask> Tasks { get; }

    void AddTask(UnitTask task);

    void SetTask(UnitTask task);
}