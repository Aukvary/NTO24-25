using System.Linq;

namespace NTO24.UI
{
    public class InventoryHUD : Drawable
    {
        private ItemCellUI[] _itemCells;

        private IInventoriable _inventory;

        private IDropable _dropable;

        public IInventoriable Inventory => _inventory;
        public IDropable Dropable => _dropable;


        protected override void Awake()
        {
            base.Awake();

            _itemCells = GetComponentsInChildren<ItemCellUI>();
        }

        public void SetEntity(IInventoriable inventory)
        {
            _dropable = null;

            _inventory?.OnItemsChangeEvent.RemoveListener(UpdateEntity);
            inventory?.OnItemsChangeEvent.AddListener(UpdateEntity);

            _inventory = inventory;

            if (inventory == null)
                foreach (var cell in _itemCells)
                    cell.Source = null;
            else
                UpdateEntity();
        }

        public void SetEntity(IDropable dropable)
        {
            _inventory?.OnItemsChangeEvent.RemoveListener(UpdateEntity);
            _inventory = null;

            _dropable = dropable;

            if (_dropable == null)
            {
                foreach (var cell in _itemCells)
                    cell.Source = null;
                return;
            }

            for (int i = 0; i < _itemCells.Length && i < Dropable.Resources.Count(); i++)
                _itemCells[i].Source = Dropable.Resources
                    .Select(p => new Pair<Resource, int>((Resource)p.Value1, p.Value2)).ElementAt(i);
        }

        private void UpdateEntity()
        {
            for (int i = 0; i < _itemCells.Length; i++)
            {
                if (i < Inventory.Items.Count())
                    _itemCells[i].Source = Inventory.Items.ElementAt(i);
                else
                    _itemCells[i].Source = null;
            }
        }
    }
}