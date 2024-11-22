using System.Collections.Generic;
using UnityEngine;

public class ConstructionObject : ActionObject
{
    [System.Serializable]
    private class ResourseCountPair
    {
        public Resource Resource;
        public uint Count;
    }

    [SerializeField]
    private List<ResourseCountPair> _materials;
    public override void Interact(Unit unit)
    {
        
    }
}