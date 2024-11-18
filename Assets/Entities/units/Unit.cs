using System.Threading.Tasks;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float _strength;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _damage;

    private UnitMoveController _moveController;

    private Vector3 _position;
    private UnitBehaviour _behavior;
    public UnitList _unitList;

    public UnitStates UnitState { get; private set; }
    public UnitBehaviour Behavior 
    {
        get => _behavior;
        set
        {
            if (_behavior == value)
                return;
            Behavior?.BehaviourExit();
            value.BehaviourEnter();
            _behavior = value;
        }
    }

    public float Strength => _strength;

    public float Speed => _speed;

    public float Damage => _damage;


    public void Awake()
    {
        Spawn(_unitList);
    }

    public void Spawn(UnitList list)
    {
        list.Add(this);
        _behavior = _moveController;
        _moveController = new(this, _speed);
    }

    public void MoveTo(Vector3 newPostion)
    {
        _moveController.TargetPosition = newPostion;
        UnitState = UnitStates.Walk;
    }

    public async void Extract(ResourceObjectSpawner spawner)
    {
        MoveTo(spawner.transform.position);
        while (Vector3.Distance(_position, spawner.transform.position) > 2)
            await Task.Delay(1);
        UnitState = UnitStates.Extraction;
    }

    public async void Attack(Unit unit)
    {
        MoveTo(unit.transform.position);
        while (Vector3.Distance(_position, unit.transform.position) > 2)
            await Task.Delay(1);

        UnitState = UnitStates.Fight;
    }
}