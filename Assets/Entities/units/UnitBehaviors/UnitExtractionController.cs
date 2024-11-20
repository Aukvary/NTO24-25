using UnityEngine;

public class UnitExtractionController : UnitBehaviour
{
    private ResourceObjectSpawner _resource;
    public ResourceObjectSpawner Resource
    {
        get => _resource;
        set
        {
            _resource = value;
            Unit.StopCoroutine(StartExtracting());
            Unit.Behavior = value == null ? null : this;
            if (value == null)
                return;
            Unit.StartCoroutine(StartExtracting());
        }
    }
    public override UnitStates UnitState => UnitStates.Extraction;
    public UnitExtractionController(Unit unit) : 
        base(unit) { }

    private System.Collections.IEnumerator StartExtracting()
    {
        while (Resource != null) 
        {
            Resource.Interact(Unit);
            if (!Resource.IsRestored)
                Resource = null;

            yield return new WaitForSeconds(Unit.AttackDelay);
        }
        yield return null;
    }

    public override void BehaviourExit()
    {
        _resource = null;
        Unit.StopCoroutine(StartExtracting());
    }
}