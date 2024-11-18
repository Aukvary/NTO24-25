using UnityEngine;

public class UnitExtractionController : UnitBehaviour
{
    private ResourceObjectSpawner _resource;
    public ResourceObjectSpawner Resource
    {
        get => _resource;
        private set
        {

        }
    }
    public override UnitStates UnitState => UnitStates.Extraction;
    public UnitExtractionController(Unit unit) : 
        base(unit) { }

    public override void BehaviourEnter()
    {
        base.BehaviourEnter();
    }

    public override void BehaviourExit()
    {
        
    }
}