using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : UnitBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private float _range;
    private float _angle;

    private Unit _attackedUnit;
    private BreakeableObject _breakeableObject;

    private RaycastHit _targetHit
    {
        get
        {
            Ray ray = new(Unit.transform.position, Vector3.up + _targetTransform.position - Unit.transform.position);
            var hit = Physics.RaycastAll(ray, _range).FirstOrDefault(hit => hit.transform == _targetTransform);
            return hit;
        }
    }

    public Unit AttackedUnit
    {
        get => _attackedUnit;

        set
        {
            _attackedUnit = value;

            Unit.Behaviour = value == null ? null : this;

        }
    }


    public BreakeableObject BreakeableObject
    {
        get => _breakeableObject;

        set
        {
            _breakeableObject = value;

            Unit.Behaviour = value == null ? null : this;
        }
    }

    private Transform _targetTransform => _attackedUnit == null ? _breakeableObject?.transform : _attackedUnit.transform;

    private bool _hasPath => Vector3.Distance(_targetHit.point, Unit.transform.position) > _range;

    public AttackBehaviour(Unit unit, float range, float angle) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
        _range = range;
        _angle = angle;
    }

    private void Attack()
    {
        if (AttackedUnit == null)
            AttackThrone(BreakeableObject);
        else
            AttackUnit(AttackedUnit);
    }

    private void AttackThrone(BreakeableObject obj)
    {
        obj.Interact(Unit);
        if (obj.Health <= 0)
            BreakeableObject = null;
    }

    private void AttackUnit(Unit unit)
    {
        if (!unit.Alive)
            AttackedUnit = null;

        var hit = _targetHit;

        if (_hasPath)
        {
            _navMeshAgent.destination = _targetTransform.position;
            return;
        }

        if (hit.collider == null)
            return;


        if (Vector3.Angle(Unit.transform.forward, hit.point - Unit.transform.position) > _angle)
            return;


        if (unit.DamageUnit(Unit, out var res))
            foreach (var item in res)
                for (int i = 0; i < item.Count; i++)
                    Unit.Inventory.TryToAdd(item.Resource);
    }

    public override void BehaviourEnter()
    {
        Unit.BehaviourAnimation.OnPunchAnimationEvent += Attack;

        _navMeshAgent.destination = _targetTransform.position;
    }

    public override void BehaviourUpdate()
    {
        if (_attackedUnit != null &&  !_attackedUnit.Alive)
            _attackedUnit = null;
        if (_targetTransform == null)
        {
            Unit.Behaviour = null;
            return;
        }


        Unit.Animator.SetTrigger(_hasPath ? "move" : "punch");


        if (!_hasPath)
        {
            _navMeshAgent.ResetPath();
        }



        if (_hasPath)
            return;
        var direction = _targetTransform.position - Unit.transform.position;
        direction.y = Unit.transform.position.y;

        var angle = Quaternion.LookRotation(direction);
        Unit.transform.rotation = Quaternion.RotateTowards(
            Unit.transform.rotation,
            angle,
            Time.deltaTime * _navMeshAgent.angularSpeed
            );
    }

    public override void BehaviourExit()
    {
        Unit.BehaviourAnimation.OnPunchAnimationEvent -= Attack;
        _navMeshAgent.ResetPath();
    }
}