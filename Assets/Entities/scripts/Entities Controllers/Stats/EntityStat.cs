using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    [Serializable]
    public class EntityStat
    {
        public static readonly Dictionary<string, EntityStatsType> StringStatPairs =
            Enum.GetValues(typeof(EntityStatsType)).Cast<EntityStatsType>().ToDictionary(s => s.ToString(), s => s);

        [SerializeField]
        private EntityStatsType _stat;

        [SerializeField] 
        private List<float> _statValues;

        [SerializeField]
        private UnityEvent _onLevelChangeEvent;

        private int _currentLevel;

        public float StatValue => _statValues[CurrentLevel];
        public int MaxLevel => _statValues.Count - 1;

        public EntityStatsType Stat => _stat;

        public int CurrentLevel
        {
            get => _currentLevel;

            set
            {
                _currentLevel = Mathf.Clamp(value, 0, MaxLevel);
                _onLevelChangeEvent.Invoke();
            }
        }

        public bool CanUpgrade => _currentLevel < MaxLevel;

        public void AddOnLevelChangeAction(UnityAction action)
            => _onLevelChangeEvent.AddListener(action);

        public void RemoveOnLevelChangeAction(UnityAction action)
            => _onLevelChangeEvent.RemoveListener(action);
    }
}