using UnityEngine;

namespace NTO24
{
    public interface IRestoreable : IEntity
    {
        public RestoreController RestoreController { get; }

        Coroutine StartRestoring() 
            => RestoreController.StartRestoring();
    }
}