using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    [CreateAssetMenu(fileName = "New Upgrade Type", menuName = "Upgrade Types")]
    public class UpgradeType : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField, TextArea]
        public string Description { get; private set; }

        [SerializeField]
        private List<EntityStatsType> _statsType; 

        [SerializeField]
        private List<List<Pair<Resource, int>>> _materials;

        public IEnumerable<EntityStatsType> StatsTypes => _statsType;

        public IEnumerable<List<Pair<Resource, int>>> Materials => _materials;
    }
}