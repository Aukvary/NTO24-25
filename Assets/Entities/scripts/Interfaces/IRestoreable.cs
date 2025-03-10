using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public interface IRestoreable : IEntity
    {
        public RestoreController RestoreController { get; }

        public int Time => RestoreController.Time;

        public UnityEvent OnTimeChange => RestoreController.OnTimeChange;

        Coroutine StartRestoring() 
            => RestoreController.StartRestoring();
    }
}