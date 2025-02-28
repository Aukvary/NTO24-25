using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class ConstructionObject : Entity, IInteractable
    {
        [SerializeField]
        private List<Pair<Resource, int>> _materials;

        [field: SerializeField]
        public UnityEvent<IInteractor> OnInteracEvent { get; private set; }

        public bool Interactable => throw new System.NotImplementedException();

        public IEnumerable<Pair<Resource, int>> Materials => _materials;
    }
}