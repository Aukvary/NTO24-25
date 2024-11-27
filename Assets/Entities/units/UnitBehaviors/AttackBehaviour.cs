using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class AttackBehaviour : UnitBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private float _range;
    private float _angle;

    private Unit _attackedUnit;
    private BreakeableObject _breakeableObject;

    private Transform _targetTransform;

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

            if (value != null)
            {
                Unit.Behaviour = this;
                Unit.BehaviourAnimation.OnPunchAnimationEvent += Attack;
            }
            else
            {
                Unit.Behaviour = null;
                Unit.BehaviourAnimation.OnPunchAnimationEvent -= Attack;
            }

        }
    }


    public BreakeableObject BreakeableObject
    {
        get => _breakeableObject;

        set
        {
            _breakeableObject = value;

            if (value != null)
            {
                if (value != null)
                {
                    Unit.Behaviour = this;
                    Unit.BehaviourAnimation.OnPunchAnimationEvent += Attack;
                }
                else
                {
                    Unit.Behaviour = null;
                    Unit.BehaviourAnimation.OnPunchAnimationEvent -= Attack;
                }
            }
        }
    }

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

    }

    private void AttackUnit(Unit unit)
    {
        if (!unit.Alive)
            AttackedUnit = null;

        var hit = _targetHit;

        if (hit.collider == null)
            return;

        if (Vector3.Angle(Unit.transform.forward, hit.point - Unit.transform.position) > _angle)
            return;


        unit.Health -= Unit.Damage;
    }

    public override void BehaviourEnter()
    {
        _targetTransform = _attackedUnit == null ? _breakeableObject.transform : _attackedUnit.transform;

        _navMeshAgent.destination = _targetTransform.position;
        Unit.Animator.SetTrigger("move");
    }

    public override void BehaviourUpdate()
    {
        if (_attackedUnit == null || !_attackedUnit.Alive)
        {
            _targetTransform = Unit.IsBee ? BreakeableObject.transform : null;
        }

        if (_targetTransform == null)
        {
            Unit.BehaviourAnimation.OnPunchAnimationEvent -= Attack;
            Unit.Behaviour = null;
            Unit.Animator.SetTrigger("idle");
            return;
        }

        if (_navMeshAgent.hasPath)
        {
            if (Vector3.Distance(_targetHit.point, Unit.transform.position) <= _range)
                _navMeshAgent.ResetPath();
            return;
        }

        Unit.Animator.SetTrigger("punch");


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
        _navMeshAgent.ResetPath();
        Unit.Animator.SetTrigger("idle");
    }
}