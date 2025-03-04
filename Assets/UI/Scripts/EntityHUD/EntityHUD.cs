using System;

namespace NTO24.UI
{
    public class EntityHUD : Drawable
    {
        private InventoryHUD _inventoryHUD;
        private HealthHUD _healthHUD;
        private IconHUD _iconHUD;

        private StatCellUI[] _statCells;

        private AnimatedUI _uiAnimator;

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
            selector.AddSelectAction(SelectEntity);

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
            if (entity is IInventoriable inventory)
                _inventoryHUD.SetEntity(inventory);
            else
                _inventoryHUD.SetEntity(null as IInventoriable);


            if (entity is IIconable icon)
                _iconHUD.Entity = icon;
            else
                _iconHUD.Entity = null;

            if (entity is IHealthable health)
                _healthHUD.Entity = health;
            else
                _healthHUD.Entity = null;

            if (entity is IDropable dropable)
                _inventoryHUD.SetEntity(dropable);

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
        }
    }
}