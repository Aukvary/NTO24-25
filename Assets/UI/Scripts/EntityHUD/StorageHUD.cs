using UnityEngine;

namespace NTO24.UI
{
    public class StorageHUD : Drawable
    {
        private InventoryHUD _inventoryHUD;
        private AnimatedUI _uiAnimator;

        private Storage _storage;

        protected override void Awake()
        {
            base.Awake();
            _inventoryHUD = GetComponent<InventoryHUD>();
            _uiAnimator = GetComponent<AnimatedUI>();
        }

        public void Initialize(Storage storage)
        {

            _storage = storage;
            _inventoryHUD.SetEntity(_storage);


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