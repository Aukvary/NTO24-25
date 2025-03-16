using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class TaskController : EntityComponent
    {
        [field: SerializeField]
        public UnityEvent<IUnitTask> OnAddEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<IUnitTask> OnSetEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<IUnitTask> OnEnterEvent { get; private set; }

        [field: SerializeField]
        public UnityEvent<IUnitTask> OnExitEvent { get; private set; }

        private IAnimationable _animable;

        private Queue<IUnitTask> _tasks;

        public IEnumerable<IUnitTask> Tasks => _tasks;

        public IUnitTask CurrentTask { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _animable = Entity is IAnimationable anim ? anim : null;
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
            if (CurrentTask != null)
                _animable?.SetAnimation(CurrentTask.Animation);
            else
                _animable?.SetAnimation(AnimationController.Animations.Idle);

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
            OnSetEvent.Invoke(task);
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
            OnAddEvent.Invoke(task);
            _tasks.Enqueue(task);
        }

        public void AddTask(IEnumerable<IUnitTask> tasks)
        {
            if (tasks.Count() == 0)
                return;

            if (_tasks.Count > 0)
                foreach (var task in tasks)
                {
                    OnAddEvent.Invoke(task);
                    _tasks.Enqueue(task);
                }
            else
            {
                SetTask(tasks.First());

                foreach (var task in tasks.Skip(1))
                {
                    OnAddEvent.Invoke(task);
                    _tasks.Enqueue(task);
                }
            }
        }

        private IUnitTask NextTask()
        {
            IUnitTask task = _tasks.Dequeue();
            task.Exit();
            OnExitEvent.Invoke(task);

            if (_tasks.TryPeek(out var t))
            {
                t.Enter();
                OnEnterEvent.Invoke(task);
                CurrentTask = t;
            }
            else
            {
                CurrentTask = null;
            }

            return task;
        }
    }
}