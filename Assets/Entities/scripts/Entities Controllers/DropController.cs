using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    public class DropController : EntityComponent
    {
        [SerializeField]
        private List<Pair<PickableObject, int>> _items;

        [SerializeField]
        private Transform _dropPosition;

        [SerializeField]
        private float _radius;

        [SerializeField]
        private float _strength;

        public IEnumerable<Pair<PickableObject, int>> Resources 
            => _items;

        public void Drop()
        {
            for (int i = 0; i < _items.Count; i++)
                _items[i].Value1.Drop(
                    _items[i].Value2,
                    _dropPosition.position,
                    _radius,
                    _strength
                    );
        }
    }
}