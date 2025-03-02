using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class TaskController : EntityComponent
    {
        [SerializeField]
        private UnityEvent<IUnitTask> _onAddEvent;

        [SerializeField]
        private UnityEvent<IUnitTask> _onSetEvent;

        [SerializeField]
        private UnityEvent<IUnitTask> _onEnterEvent;

        [SerializeField]
        private UnityEvent<IUnitTask> _onExitEvent;

        private Queue<IUnitTask> _tasks;

        public IEnumerable<IUnitTask> Tasks => _tasks;

        public IUnitTask CurrentTask { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _tasks = new Queue<IUnitTask>();
        }

        protected override void Update()
        {
            CurrentTask?.Update();
            if (CurrentTask != null && CurrentTask.IsComplete)
                NextTask();
        }

        protected override void FixedUpdate()
        {
            CurrentTask?.FixedUpdate();
        }

        public void SetTask(IUnitTask task)
        {
            if (_tasks.TryPeek(out var t))
                t.Exit();

            _tasks.Clear();
            _tasks.Enqueue(task);
            task.Enter();
            CurrentTask = task;
            _onSetEvent.Invoke(task);
        }

        public void SetTask(IEnumerable<IUnitTask> tasks)
        {
            if (tasks.Count() == 0)
                return;

            SetTask(tasks.First());

            foreach (var task in tasks.Skip(1))
                _tasks.Enqueue(task);
        }

        public void AddTask(IUnitTask task)
        {
            _tasks.Enqueue(task);
            _onAddEvent.Invoke(task);
        }

        public void AddTask(IEnumerable<IUnitTask> tasks)
        {
            foreach (var task in tasks)
                _tasks.Enqueue(task);
        }

        private IUnitTask NextTask()
        {
            IUnitTask task = _tasks.Dequeue();
            task.Exit();
            _onExitEvent.Invoke(task);


            if (_tasks.TryPeek(out var t))
            {
                t.Enter();
                _onEnterEvent.Invoke(task);
                CurrentTask = t;
            }
            else
                CurrentTask = null;

            return task;
        }

        public void AddOnAddAction(UnityAction<IUnitTask> action)
            => _onAddEvent.AddListener(action);

        public void RemoveOnAddAction(UnityAction<IUnitTask> action)
            => _onAddEvent.RemoveListener(action);

        public void AddOnSetAction(UnityAction<IUnitTask> action)
            => _onSetEvent.AddListener(action);

        public void RemoveOnSetAction(UnityAction<IUnitTask> action)
            => _onSetEvent.RemoveListener(action);

        public void AddOnEnterAction(UnityAction<IUnitTask> action)
            => _onEnterEvent.AddListener(action);

        public void RemoveEnterAction(UnityAction<IUnitTask> action)
            => _onEnterEvent.RemoveListener(action);

        public void AddOnExitAction(UnityAction<IUnitTask> action)
            => _onExitEvent.AddListener(action);

        public void RemoveOnExitAction(UnityAction<IUnitTask> action)
            => _onExitEvent.RemoveListener(action);
    }
}