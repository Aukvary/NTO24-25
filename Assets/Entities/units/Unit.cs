using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float _strength;

    [SerializeField, Min(0f)]
    private float _speed;

    [SerializeField, Min(0f)]
    private float _damage;

    [SerializeField, Min(0f)]
    private float _attackDelay;

    private UnitMovementController _moveController;
    private UnitExtractionController _extractionController;

    private UnitBehaviour _behavior;
    public UnitList _unitList;

    public UnitStates UnitState { get; private set; }
    public UnitBehaviour Behavior 
    {
        get => _behavior;
        set
        {
            if (Behavior == value)
                return;
            Behavior?.BehaviourExit();
            value?.BehaviourEnter();
            _behavior = value;
        }
    }

    public float Strength => _strength;

    public float Speed => _speed;

    public float Damage => _damage;

    public float AttackDelay => _attackDelay;

    private Vector3 _position => transform.position;

    public void Awake()
    {
        Spawn(_unitList);
    }

    public void Spawn(UnitList list)
    {
        list.Add(this);
        _behavior = _moveController;
        _moveController = new(this, _speed);
        _extractionController = new(this);
    }

    public void MoveTo(Vector3 newPostion)
    {
        _moveController.TargetPosition = newPostion;
    }

    public async void Extract(ResourceObjectSpawner spawner)
    {
        MoveTo(spawner.transform.position);

        while (!_moveController.HasPath)
            await Task.Delay(1);

        while (_moveController.HasPath)
            await Task.Delay(1);

        _extractionController.Resource = spawner;
    }

    public void Follow(Unit unit)
    {
        _moveController.FollowUnit = unit;
    }

    public async void Attack(Unit unit)
    {
        MoveTo(unit.transform.position);
        while (Vector3.Distance(_position, unit.transform.position) > 2)
            await Task.Delay(1);

        
    }
}