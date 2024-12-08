using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable CS4014
public class ConstructionObject : MonoBehaviour, IInteractable, ILoadable
{
    [System.Serializable]
    public class RepairInfo
    {
        public Resource Resource;
        [Min(0)]
        public int Count;
        public TextMeshPro _countText;
    }

    [SerializeField]
    private List<RepairInfo> _materials;

    [SerializeField]
    private UnityEvent _onBuildEvent;

    [SerializeField]
    private string _name;

    private readonly string _repair = "repair";

    private User _repairUser;

    public IEnumerable<RepairInfo> Materials => _materials;

    public bool Loaded { get; set; }

    public Transform Transform => transform;

    private void Awake()
    {
        foreach (var pair in _materials)
        {
            pair._countText.text = pair.Count.ToString();
        }

        _repairUser = new(_name);

        Initialize();
    }
    public bool CanInteract(Unit unit)
        => _materials.Any(material => (unit as Bear).Storage[material.Resource] > 0 && material.Count != 0);

    public void Interact(Unit unit)
    {
        if (unit is Bee)
            return;

        var bear = unit as Bear;
        foreach (var pair in _materials)
        {
            if (bear.Storage[pair.Resource] == 0)
                continue;

            if (pair.Count == 0)
                continue;

            pair.Count -= 1;
            bear.Storage[pair.Resource] -= 1;

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
        _onBuildEvent?.Invoke();
        try
        {
            Destroy(gameObject);
        }
        catch(MissingReferenceException) { }
    }

    public async void Initialize()
    {
        await _repairUser.InitializeUser(_repair);

        if (_repairUser.Resources[_repair] == 0)
            return;

        Build();
    }

    public void AddListnerToBuild(UnityAction action)
    {
        _onBuildEvent.AddListener(action);
    }
}