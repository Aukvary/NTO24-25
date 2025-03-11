using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class RestoreController : EntityComponent
    {
        [SerializeField]
        private float _restoreTime;

        [HideInInspector]
        public UnityEvent OnTimeChangeEvent { get; private set; } = new();

        private EntityHealth _healthController;

        private Vector3 _spawnPosition;

        public int Time { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            _spawnPosition = transform.position;

            _healthController = GetComponent<EntityHealth>();

            _healthController.OnDeathEvent.AddListener(alive =>
            {
                transform.position = _spawnPosition;
            });
        }

        public Coroutine StartRestoring()
            => StartCoroutine(Restore());
        
        private System.Collections.IEnumerator Restore()
        {
            StartCoroutine(StartTimer());
            yield return new WaitForSeconds(_restoreTime);
            _healthController.Alive = true;
        }

        private System.Collections.IEnumerator StartTimer()
        {
            Time = (int)_restoreTime;
            while(Time > 0)
            {
                OnTimeChangeEvent.Invoke();
                Time -= 1;
                yield return new WaitForSeconds(1);
            }
            OnTimeChangeEvent.Invoke();
        }
    }
}