using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public class ConstructionObject : ActionObject
{
    [System.Serializable]
    public class ResourseCountPair
    {
        public Resource Resource;
        [Min(0)]
        public int Count;
        [Min(0)]
        public int _stepCount;
        public TextMeshPro _countText;
    }

    [SerializeField]
    private List<ResourseCountPair> _materials;

    [SerializeField]
    private UnityEvent _afterBuildEvent;

    public IEnumerable<ResourseCountPair> Materials => _materials;


    private void Awake()
    {
        foreach (var pair in _materials) 
        {
            pair._countText.text = pair.Count.ToString();
        }
    }
    public override void Interact(Unit unit)
    {
        if (_materials.All(material => unit.Storage[material.Resource] == 0))
        {
            unit.Behaviour = null;
            return;
        }


        foreach (var pair in _materials)
        {
            var step = Mathf.Min(unit.Storage[pair.Resource], pair._stepCount);
            pair.Count -= step;
            unit.Storage[pair.Resource] -= step;

            pair._countText.text = pair.Count.ToString();
        }

        
        if (_materials.All(p => p.Count == 0))
        {
            _afterBuildEvent.Invoke();
            Destroy(gameObject);
        }
    }
}