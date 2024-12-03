using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public class ConstructionObject : ActionObject
{
    [System.Serializable]
    public class RepairInfo
    {
        public Resource Resource;
        [Min(0)]
        public int Count;
        [Min(0)]
        public int _stepCount;
        public TextMeshPro _countText;
    }

    [SerializeField]
    private List<RepairInfo> _materials;

    [SerializeField]
    private UnityEvent _afterBuildEvent;

    [SerializeField]
    private string _name;

    private readonly string _repair = "repair";

    private User _repairUser;

    public IEnumerable<RepairInfo> Materials => _materials;


    private void Awake()
    {
        foreach (var pair in _materials) 
        {
            pair._countText.text = pair.Count.ToString();
        }

        _repairUser = new(_name);

        Initialize();
    }

    private async void Initialize()
    {
        await _repairUser.InitializeUser(_repair);

        if (_repairUser.Resources[_repair] == 0)
            return;

        Build();
    }
    public override void Interact(Unit unit)
    {
        if (_materials.All(material => unit.Storage[material.Resource] == 0))
        {
            unit.Behaviour = null;
            return;
        }


        foreach (var pair in _materials)
        {
            var step = Mathf.Min(unit.Storage[pair.Resource], pair._stepCount);
            pair.Count -= step;
            unit.Storage[pair.Resource] -= step;

            pair._countText.text = pair.Count.ToString();
        }


        if (_materials.All(p => p.Count == 0))
        {
            _repairUser.UpdateUser(_repair, 1);
            Build();
        }
    }

    private void Build()
    {
        _afterBuildEvent?.Invoke();
        Destroy(gameObject);
    }
}