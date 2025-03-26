using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    [Serializable]
    public class EntityStat
    {
        [SerializeField]
        private StatInfo _stat;

        [SerializeField]
        public List<float> _statValues;

        [SerializeField]
        private UnityEvent _onLevelChangeEvent = new();

        [field: SerializeField]
        public UnityEvent OnUpgradeEvent { get; private set; } = new();

        private int _currentLevel;

        public float StatValue => _statValues[CurrentLevel];
        public float NextStatValue => _statValues[CurrentLevel + 1];

        public int MaxLevel => _statValues.Count - 1;

        public StatInfo StatInfo => _stat;

        public int CurrentLevel
        {
            get => _currentLevel;

            set
            {
                if (value != _currentLevel)
                    OnUpgradeEvent.Invoke();
                _currentLevel = Mathf.Clamp(value, 0, MaxLevel);
                _onLevelChangeEvent.Invoke();

            }
        }

        public bool CanUpgrade => _currentLevel < MaxLevel;

        public EntityStat(StatInfo source, float[] stats)
        {
            _stat = source;
            _statValues = stats.ToList();
        }

        public void AddOnLevelChangeAction(UnityAction action)
            => _onLevelChangeEvent.AddListener(action);

        public void RemoveOnLevelChangeAction(UnityAction action)
            => _onLevelChangeEvent.RemoveListener(action);
    }
}