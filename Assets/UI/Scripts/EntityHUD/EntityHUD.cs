using System;
using TMPro;
using UnityEngine;

namespace NTO24.UI
{
    public class EntityHUD : Drawable
    {
        [SerializeField]
        private TextMeshProUGUI _restoreTimer;

        private InventoryHUD _inventoryHUD;
        private HealthHUD _healthHUD;
        private IconHUD _iconHUD;

        private StatCellUI[] _statCells;

        private AnimatedUI _uiAnimator;

        private IRestoreable _restoreable;

        protected override void Awake()
        {
            base.Awake();
            _inventoryHUD = GetComponentInChildren<InventoryHUD>();
            _healthHUD = GetComponentInChildren<HealthHUD>();
            _iconHUD = GetComponentInChildren<IconHUD>();

            _uiAnimator = GetComponent<AnimatedUI>();

            _statCells = GetComponentsInChildren<StatCellUI>();
        }

        public void Initialize(EntitySelector selector)
        {
            selector.OnEntitySelecteEvent.AddListener(SelectEntity);

            _inventoryHUD.SetEntity(null as IInventoriable);
            _iconHUD.Entity = null;
            _healthHUD.Entity = null;

            _uiAnimator.Hide();
            _uiAnimator.Complete();
        }

        private void SelectEntity(Entity entity)
        {
            if (entity == null)
            {
                _uiAnimator.Hide(onCompleteCallBack: () =>
                {
                    _inventoryHUD.SetEntity(null as IInventoriable);
                    _iconHUD.Entity = null;
                    _healthHUD.Entity = null;
                });
                return;
            }

            _uiAnimator.Show();

            _inventoryHUD.SetEntity(entity is IInventoriable inventory ? inventory : null);
            _inventoryHUD.SetEntity(entity is IDropable dropable ? dropable : null);

            _iconHUD.Entity = entity is IIconable icon ? icon : null;

            _healthHUD.Entity = entity is IHealthable health ? health : null;

            if (entity is IStatsable statsable)
            {
                _statCells[0].Stat = statsable[StatsNames.Speed];
                _statCells[1].Stat = statsable[StatsNames.Damage];
                _statCells[2].Stat = statsable[StatsNames.InteractPower];
                _statCells[3].Stat = statsable[StatsNames.CellCapacity];
            }
            else
            {
                foreach (var cell in _statCells)
                    cell.Stat = null;
            }

            if (entity is IRestoreable resotorable)
            {
                _restoreable?.OnTimeChange.RemoveListener(UpdateTimer);
                resotorable.OnTimeChange.AddListener(UpdateTimer);
                _restoreable = resotorable;
                UpdateTimer();
            }
            else
            {
                _restoreable?.OnTimeChange.RemoveListener(UpdateTimer);
                _restoreTimer.text = "";
            }
        }

        private void UpdateTimer()
            => _restoreTimer.text = _restoreable.Time == 0 ? "" : _restoreable.Time.ToString();
    }
}