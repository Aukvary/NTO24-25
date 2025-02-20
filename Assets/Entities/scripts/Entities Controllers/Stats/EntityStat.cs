using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private UnityEvent<int> _onLevelChangeEvent;

    private int _currentLevel;

    public float StatValue => _statValues[CurrentLevel];
    public int MaxLevel => _statValues.Count;

    public EntityStatsType Stat => _stat;

    public int CurrentLevel
    {
        get => _currentLevel;

        set
        {
            _currentLevel = Mathf.Clamp(value, 0, MaxLevel);
            _onLevelChangeEvent.Invoke(_currentLevel);
        }
    }

    public bool CanUpgrade => _currentLevel < MaxLevel;

    public void AddOnLevelChangeAction(UnityAction<int> action)
        => _onLevelChangeEvent.AddListener(action);

    public void RemoveOnLevelChangeAction(UnityAction<int> action)
        => _onLevelChangeEvent.RemoveListener(action);
}