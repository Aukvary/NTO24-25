using UnityEngine;

namespace NTO24.UI
{
    public class StorageHUD : Drawable
    {
        private InventoryHUD _inventoryHUD;
        private AnimatedUI _uiAnimator;

        private Storage _storage;

        public void Initialize(Storage storage)
        {
            base.Awake();
            _inventoryHUD = GetComponent<InventoryHUD>();

            _storage = storage;
            _inventoryHUD.SetEntity(_storage);

            _uiAnimator = GetComponent<AnimatedUI>();

            _uiAnimator.Hide();
            _uiAnimator.Complete();
        }

        protected override void Update()
        {
            if (Input.GetKey(KeyCode.Tab))
                _uiAnimator.Show();
            else
                _uiAnimator.Hide();
        }
    }
}