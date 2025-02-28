using DG.Tweening;
using UnityEngine;


namespace NTO24.UI
{
    public class EntityHUD : Drawable
    {
        private InventoryHUD _inventoryHUD;
        private HealthHUD _healthHUD;
        private IconHUD _iconHUD;

        private AnimatedUI _uiAnimator;

        protected override void Awake()
        {
            base.Awake();
            _inventoryHUD = GetComponentInChildren<InventoryHUD>();
            _healthHUD = GetComponentInChildren<HealthHUD>();
            _iconHUD = GetComponentInChildren<IconHUD>();

            _uiAnimator = GetComponent<AnimatedUI>();
        }

        protected override void Start()
        {
            _inventoryHUD.SetEntity(null as IInventoriable);
            _iconHUD.Entity = null;
            _healthHUD.Entity = null;

            _uiAnimator.Hide();
            _uiAnimator.Complete();
        }

        protected override void Update()
        {
            SelectEntity();

            if (Input.GetKeyDown(KeyCode.F))
                _healthHUD.Entity?.Damage(10);

            if (Input.GetKeyDown(KeyCode.G))
                _healthHUD.Entity?.Heal(10);
        }

        private void SelectEntity()
        {
            if (/*UIRayCast ||*/ !Input.GetKeyDown(KeyCode.Mouse0))
                return;

            

            if (!Physics.Raycast(Direction, out var hit) ||
                !hit.transform.TryGetComponent<Entity>(out var entity))
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
        }
    }
}