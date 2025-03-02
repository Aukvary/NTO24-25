using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField]
        private List<UpgradeType> _upgradeTypes;

        private UnityEvent _onEntityChangeEvent = new();

        private UnityEvent _onUpgradeEvent = new();

        private int _upgradeTypeIndex;

        private IInventoriable _storage;

        private IStatsable _bear;

        public IStatsable Bear
        {
            get => _bear;

            set
            {
                _bear = value;
                _onEntityChangeEvent.Invoke();
            }
        }

        public UpgradeType UpgradeType => _upgradeTypes[_upgradeTypeIndex];

        public bool CanUpgrade => UpgradeType.Materials.ElementAt(_upgradeTypeIndex).All(p =>
            {
                return _storage[p.Value1] >= p.Value2;
            });

        public void Initialize(EntitySelector selector, Storage storage)
        {
            selector.AddListner(SelectBear);

            _storage = storage;
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


        }
    }
}