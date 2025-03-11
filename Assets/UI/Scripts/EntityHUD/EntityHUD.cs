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

        private readonly StatNames[] _displayStats =
        {
            StatNames.Damage,
            StatNames.Speed,
            StatNames.InteractPower,
            StatNames.CellCapacity
        };

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
                for (int i = 0; i < _statCells.Length; i++)
                {
                    try
                    {
                        _statCells[i].Stat = statsable[_displayStats[i]];
                    }
                    catch { _statCells[i].Stat = null; }
                }
            }
            else
            {
                foreach (var cell in _statCells)
                    cell.Stat = null;
            }

            if (entity is IRestoreable resotorable)
            {
                _restoreable?.OnTimeChangeEvent.RemoveListener(UpdateTimer);
                resotorable.OnTimeChangeEvent.AddListener(UpdateTimer);
                _restoreable = resotorable;
                UpdateTimer();
            }
            else
            {
                _restoreable?.OnTimeChangeEvent.RemoveListener(UpdateTimer);
                _restoreTimer.text = "";
            }
        }

        private void UpdateTimer()
            => _restoreTimer.text = _restoreable.Time == 0 ? "" : _restoreable.Time.ToString();
    }
}