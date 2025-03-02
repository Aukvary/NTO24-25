using System.Collections.Generic;
using UnityEngine;

namespace NTO24
{
    [CreateAssetMenu(fileName = "New Upgrade Type", menuName = "Upgrade Types")]
    public class UpgradeType : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Icon;

        [SerializeField]
        private List<EntityStatsType> _statsType; 

        [SerializeField]
        private List<List<Pair<Resource, int>>> _materials;

        public IEnumerable<EntityStatsType> StatsTypes => _statsType;

        public IEnumerable<List<Pair<Resource, int>>> Materials => _materials;
    }
}