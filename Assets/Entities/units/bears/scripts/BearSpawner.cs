using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    public class BearSpawner : UnitSpawner
    {
        [SerializeField]
        private List<Pair<Bear, Transform>> _bears;

        public IEnumerable<Bear> Spawn()
        {
            Bear[] bears = new Bear[_bears.Count];
            for (int i = 0; i < _bears.Count; i++)
            {
                bears[i] = Instantiate(_bears[i].Value1, _bears[i].Value2.position, Quaternion.identity);
                bears[i].Init(BearInfo.Bears[i]);
            }

            return bears;
        }
    }
}