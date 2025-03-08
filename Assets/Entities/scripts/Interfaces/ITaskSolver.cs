using System.Collections.Generic;
using UnityEngine.Events;

namespace NTO24
{
    public interface ITaskSolver : IEntity
    {
        TaskController TaskController { get; }

        public UnityEvent<IUnitTask> OnAddEvent => TaskController.OnAddEvent;

        public UnityEvent<IUnitTask> OnSetEvent => TaskController.OnSetEvent;

        public UnityEvent<IUnitTask> OnEnterEvent => TaskController.OnEnterEvent;

        public UnityEvent<IUnitTask> OnExitEvent => TaskController.OnExitEvent;

        void AddTask(IUnitTask task) 
            => TaskController.AddTask(task);

        void AddTask(IEnumerable<IUnitTask> tasks) 
            => TaskController.AddTask(tasks);

        void SetTask(IUnitTask task)
            => TaskController.SetTask(task);

        void SetTask(IEnumerable<IUnitTask> tasks)
            => TaskController.SetTask(tasks);
    }
}