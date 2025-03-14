using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class RestoreController : EntityComponent, ISavableComponent
    {
        [SerializeField]
        private float _restoreTime;

        [HideInInspector]
        public UnityEvent OnTimeChangeEvent { get; private set; } = new();

        public UnityEvent OnDataChangeEvent { get; private set; } = new();

        private EntityHealth _healthController;

        private Vector3 _spawnPosition;

        public int Time { get; private set; } = 0;

        public string Name => "Restore";
        public string[] Data => new string[] { Time.ToString()};

        protected override void Awake()
        {
            base.Awake();

            _spawnPosition = transform.position;

            _healthController = GetComponent<EntityHealth>();

            _healthController.OnDeathEvent.AddListener(alive =>
            {
                transform.position = _spawnPosition;
                StartRestoring();
            });
        }

        public void ServerInitialize(IEnumerable<string> data)
        {
            float time = float.Parse(data.ElementAt(0));
            if (time == 0)
                return;
            StartCoroutine(Restore(time));
        }

        public Coroutine StartRestoring()
            => StartCoroutine(Restore(_restoreTime));
        
        private IEnumerator Restore(float time)
        {
            StartCoroutine(StartTimer(time));
            yield return new WaitForSeconds(time);
            _healthController.Alive = true;
            Time = 0;
        }

        private IEnumerator StartTimer(float time)
        {
            Time = (int)time;
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