using System.Collections.Generic;
using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    [SerializeField]
    private List<UnitTransformPair<Bear>> _bears;

    public IEnumerable<Bear> Spawn()
    {
        Bear[] bears = new Bear[_bears.Count];
        for (int i = 0; i < _bears.Count; i++)
        {
            bears[i] = Instantiate(_bears[i].Unit, _bears[i].Position.position, Quaternion.identity);
        }

        return bears;
    }
}