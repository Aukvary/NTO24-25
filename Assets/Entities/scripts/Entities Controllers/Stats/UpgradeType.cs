using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    [CreateAssetMenu(fileName = "New Upgrade Type", menuName = "Upgrade Types")]
    public class UpgradeType : ScriptableObject
    {
        [Serializable]
        public class UpgradeMaterials
        {
            [SerializeField]
            private List<Pair<Resource, int>> _materials;

            public IEnumerable<Pair<Resource, int>> Materials => _materials;
        }

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField, TextArea]
        public string Description { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }

        [SerializeField]
        private List<StatsNames> _statsType; 

        [SerializeField]
        private List<UpgradeMaterials> _materials;

        public IEnumerable<StatsNames> StatsTypes => _statsType;

        public IEnumerable<UpgradeMaterials> Materials => _materials;
    }
}