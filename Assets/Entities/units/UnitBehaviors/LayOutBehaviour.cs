using System.Linq;
using UnityEngine;

public class LayOutBehaviour : UnitBehaviour
{
    private Storage _storage;

    private RaycastHit _targetHit
    {
        get
        {
            Ray ray = new(Unit.transform.position, Vector3.up + _storage.transform.position - Unit.transform.position);
            var hit = Physics.RaycastAll(ray, Range).FirstOrDefault(hit => hit.transform == _storage.transform);
            return hit;
        }
    }

    private bool _hasPath
        => _targetHit.collider == null ? true : Vector3.Distance(_targetHit.point, Unit.transform.position) > Range;

    public Storage Storage
    {
        get => _storage;

        set
        {
            _storage = value;

            Unit.Behaviour = value == null ? null : this;
        }
    }

    public LayOutBehaviour(Unit unit, float range) : 
        base(unit, range) { }

    public override void BehaviourEnter()
    {
        NavMeshAgent.destination = Storage.transform.position;
    }

    public override void BehaviourUpdate()
    {
        Unit.Animator.SetTrigger(_hasPath ? "move" : "idle");
        if (_hasPath)
            return;

        Storage.Interact(Unit);
        Storage = null;
    }

    public override void BehaviourExit()
    {
        NavMeshAgent.ResetPath();
    }
}