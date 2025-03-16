using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class UnitSpawner : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<Unit> OnSpawnEvent { get; private set; }
    }
}
