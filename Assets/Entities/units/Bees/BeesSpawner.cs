using System.Collections.Generic;
using UnityEngine;

public class BeesSpawner : MonoBehaviour
{
    /*[System.Serializable]
    private struct UnitTransform
    {
        [SerializeField]
        private Unit _unit;
        [SerializeField]
        private Transform _transform;

        public Unit Unit => _unit;
        public Transform Transform => _transform;
    }*/
    [SerializeField]
    private List<Transform> values;

    [SerializeField]
    private BeeActivityController _unit;

    [SerializeField]
    private Throne _durovHome;
}