using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Bear : Unit, IInventoriable, ILoadable
{
    [SerializeField]
    private UnityEvent _onUserInitializeEvent;

    [SerializeField]
    private float _restoreTime;

    [SerializeField]
    private Storage _storage;

    [SerializeField]
    private Sprite _headSprite;

    [SerializeField]
    private string _unitName;

    private Collider _collider;
    private MeshRenderer[] _renderers;

    private Inventory _inventory;

    private Vector3 _spawnPosition;

    public Sprite HeadSprite => _headSprite;

    public Storage Storage => _storage;

    public string Name => _unitName;


    public Inventory Inventory => _inventory;

    public bool CanAppend => throw new NotImplementedException();
    public int this[Resource resource] => throw new NotImplementedException();

    public bool IsInitialized { get; set; }
    public UnityEvent OnUserInitializeEvent => _onUserInitializeEvent;
    public User User { get; private set; }
    



    public event UnityAction<IInventoriable, ResourceCountPair> OnFailedAddEvent;

    public Bear Spawn(Vector3 position, Storage storage)
    {
        var bear = Instantiate(this, position, Quaternion.identity);

        bear._storage = storage;
        bear._spawnPosition = position;

        return bear;
    }

    protected override void Awake()
    {
        base.Awake();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _collider = GetComponent<Collider>();

    }

    public async Task Initialize()
    {

    }

    public IEnumerable<string> GetStringParametors()
        => Stats.Select(s => s.Stat.ToString());

    protected override void InitializeHealth()
    {
        base.InitializeHealth();
        HealthComponent.AddOnDeathAction(entity =>
        {
            StartCoroutine(StartRestore());
        });

        HealthComponent.AddOnAliveChangeAction(alive =>
        {
            _collider.enabled = alive;

            foreach (var renderer in _renderers)
                renderer.enabled = alive;
        });
    }

    public void AddToInventory(ResourceCountPair resources)
    {

    }

    public void AddToInventory(IEnumerable<ResourceCountPair> resources)
    {

    }

    public void UpdateUserInfo(string parametor, int value)
    {

    }

    private IEnumerator StartRestore()
    {
        yield return new WaitForSeconds(_restoreTime);
        HealthComponent.Alive = true;
    }
}