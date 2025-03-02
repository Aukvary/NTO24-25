using System.Collections.Generic;
using UnityEngine.Events;

namespace NTO24
{
    public interface ITaskSolver : IEntity
    {
        TaskController TaskController { get; }

        void AddTask(IUnitTask task) 
            => TaskController.AddTask(task);

        void AddTask(IEnumerable<IUnitTask> tasks) 
            => TaskController.AddTask(tasks);

        void SetTask(IUnitTask task)
            => TaskController.SetTask(task);

        void SetTask(IEnumerable<IUnitTask> tasks)
            => TaskController.SetTask(tasks);

        void AddOnAddAction(UnityAction<IUnitTask> action)
            => TaskController.AddOnAddAction(action);

        void RemoveOnAddAction(UnityAction<IUnitTask> action)
            => TaskController.RemoveOnAddAction(action);

        void AddOnSetAction(UnityAction<IUnitTask> action)
            => TaskController.AddOnSetAction(action);

        void RemoveOnSetAction(UnityAction<IUnitTask> action)
            => TaskController.RemoveOnSetAction(action);

        void AddOnEnterAction(UnityAction<IUnitTask> action)
            => TaskController.AddOnEnterAction(action);

        void RemoveEnterAction(UnityAction<IUnitTask> action)
            => TaskController.RemoveEnterAction(action);

        void AddOnExitAction(UnityAction<IUnitTask> action)
            => TaskController.AddOnExitAction(action);

        void RemoveOnExitAction(UnityAction<IUnitTask> action)
            => TaskController.RemoveOnExitAction(action);
    }
}