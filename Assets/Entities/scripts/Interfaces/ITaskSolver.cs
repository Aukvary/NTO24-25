using System.Collections.Generic;

public interface ITaskSolver : IEntity
{
    IEnumerable<IUnitTask> Tasks { get; }
    IUnitTask CurrentTask { get; }

    void AddTask(IUnitTask task);
    void AddTask(IEnumerable<IUnitTask> tasks);

    void SetTask(IUnitTask task);
    void SetTask(IEnumerable<IUnitTask> tasks);
}