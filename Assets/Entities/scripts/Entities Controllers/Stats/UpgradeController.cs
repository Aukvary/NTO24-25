using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class UpgradeController : MonoBehaviour
    {
        private UpgradeType[] _upgradeTypes;

        private UpgradeType _upgradeType;

        public UnityEvent OnEntityChangeEvent { get; private set; } = new();

        public UnityEvent OnUpgradeEvent { get; private set; } = new();

        public UnityEvent OnChangeTypeEvent { get; private set; } = new();

        public IInventoriable Storage { get; private set; }

        private IStatsable _bear;

        public IStatsable Bear
        {
            get => _bear;

            set
            {
                _bear = value;
                OnEntityChangeEvent.Invoke();
            }
        }

        public int CurrentLevel => Bear[UpgradeType.StatsTypes.First()].CurrentLevel;
        public int MaxLevel => Bear[UpgradeType.StatsTypes.First()].MaxLevel;

        public IEnumerable<Pair<Resource, int>> Materials 
            => UpgradeType.Materials.ElementAt(CurrentLevel).Materials;

        public UpgradeType UpgradeType 
        {
            get => _upgradeType;

            set
            {
                _upgradeType = value;
                OnChangeTypeEvent.Invoke();
            }
        }

        public IEnumerable<UpgradeType> UpgradeTypes => _upgradeTypes;

        public bool CanUpgrade => CurrentLevel < MaxLevel && Materials.All(p =>
            {
                return Storage[p.Value1] >= p.Value2;
            });

        private void Awake()
        {
            _upgradeTypes = UnityEngine.Resources.LoadAll<UpgradeType>("UpgradeTypes");
        }

        public void Initialize(EntitySelector selector, Storage storage, Bear bear)
        {
            UpgradeType = _upgradeTypes.First();

            selector.AddSelectAction(SelectBear);
            SelectBear(bear);

            Storage = storage;
        }

        private void SelectBear(Entity entity)
        {
            if (entity is not Bear bear)
                return;

            Bear = bear;
        }

        public void TryUpgrade()
        {
            if (!CanUpgrade)
                return;

            foreach (var pair in Materials)
            {
                Storage.RemoveResources(pair.Value1, pair.Value2);
            }

            foreach (var stat in UpgradeType.StatsTypes)
                Bear[stat].CurrentLevel++;

            OnUpgradeEvent.Invoke();
        }
    }
}