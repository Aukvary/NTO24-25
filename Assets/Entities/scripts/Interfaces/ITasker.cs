using System.Collections.Generic;

public interface ITasker : IEntity
{
    IEnumerable<IUnitTask> Tasks { get; }

    void AddTask(IUnitTask task);

    void SetTask(IUnitTask task);
}