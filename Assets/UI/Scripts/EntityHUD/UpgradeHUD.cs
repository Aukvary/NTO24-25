using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class UpgradeHUD : Drawable
    {
        [SerializeField]
        private Color _canColor;

        [SerializeField]
        private Color _cantColor;

        [SerializeField]
        private Button _upgradeButton;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _description;

        private Image _upgradeButtonImage;

        private AnimatedUI _uiAnimator;

        private IconHUD _iconHUD;

        private UpgradeController _upgradeController;

        private UpgradeTypeButton[] _buttons;

        private ItemCellUI[] _materialCells;

        private StatChangesField[] _changeFields;

        private IEnumerable<UpgradeType> UpgradeTypes => _upgradeController.UpgradeTypes;

        protected override void Awake()
        {
            _uiAnimator = GetComponent<AnimatedUI>();
            _buttons = GetComponentsInChildren<UpgradeTypeButton>();
            _iconHUD = GetComponentInChildren<IconHUD>();
            _upgradeButtonImage = _upgradeButton.GetComponent<Image>();
            _materialCells = GetComponentsInChildren<ItemCellUI>();
            _changeFields = GetComponentsInChildren<StatChangesField>();
        }

        public void Initialize(UpgradeController controller)
        {
            _upgradeController = controller;

            InitializeIcon();
            InitializeTypeButtons();
            InitializeUpgradeButton();
            InitializeMaterialCells();
            InitializeChangeFields();

            _uiAnimator.Hide();
            _uiAnimator.Complete();
        }

        private void InitializeIcon()
        {
            _upgradeController.OnEntityChangeEvent.AddListener(() => {
                _iconHUD.Entity = _upgradeController.Bear as IIconable;
            });
            _iconHUD.Entity = _upgradeController.Bear as IIconable;
        }

        private void InitializeTypeButtons()
        {
            _upgradeController.OnChangeTypeEvent.AddListener(() =>
            {
                _title.text = _upgradeController.UpgradeType.Name;
                _description.text = _upgradeController.UpgradeType.Description;
            });

            for (int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[i].UpgradeType = UpgradeTypes.ElementAt(i);

                _buttons[i].OnClick.AddListener(() =>
                {
                    foreach (var button in _buttons)
                        button.IsSelected(false);

                    _upgradeController.UpgradeType = _buttons[index].IsSelected(true);
                });
            }
            _upgradeController.UpgradeType = _buttons[0].IsSelected(true);
        }

        private void InitializeMaterialCells()
        {
            _upgradeController.OnEntityChangeEvent.AddListener(() =>
            {
                for (int i = 0; i < _materialCells.Length; i++)
                    UpdateMaterials(i);
            });

            _upgradeController.OnChangeTypeEvent.AddListener(() =>
            {
                for (int i = 0; i < _materialCells.Length; i++)
                    UpdateMaterials(i);
            });

            _upgradeController.OnEntityChangeEvent.AddListener(() =>
            {
                for (int i = 0; i < _materialCells.Length; i++)
                    UpdateMaterials(i);
            });

            for (int i = 0; i < _materialCells.Length; i++)
                UpdateMaterials(i);
        }

        private void UpdateMaterials(int index)
        {
            if (_upgradeController.CurrentLevel == _upgradeController.MaxLevel ||
                index >= _upgradeController.Materials.Count())
                _materialCells[index].Source = null;
            else
                _materialCells[index].Source = _upgradeController.Materials.ElementAt(index);
        }

        private void InitializeUpgradeButton()
        {
            _upgradeButton.onClick.AddListener(_upgradeController.TryUpgrade);


            _upgradeController.OnEntityChangeEvent.AddListener(UpdateColor);

            _upgradeController.OnUpgradeEvent.AddListener(UpdateColor);

            _upgradeController.OnChangeTypeEvent.AddListener(UpdateColor);

            _upgradeController.Storage.OnItemsChangeEvent.AddListener(UpdateColor);

            _upgradeButtonImage.color = _upgradeController.CanUpgrade ? _canColor : _cantColor;
        }

        private void UpdateColor()
        {
            float a = _upgradeButtonImage.color.a;
            _upgradeButtonImage.color = _upgradeController.CanUpgrade ? _canColor : _cantColor;
            Color color = _upgradeButtonImage.color;
            color.a = a;
            _upgradeButtonImage.color = color;
        }

        private void InitializeChangeFields()
        {
            _upgradeController.OnEntityChangeEvent.AddListener(UpdateChangesFields);
            _upgradeController.OnEntityChangeEvent.AddListener(UpdateChangesFields);
            _upgradeController.OnUpgradeEvent.AddListener(UpdateChangesFields);

            UpdateChangesFields();
        }

        private void UpdateChangesFields()
        {
            var stats = _upgradeController.Bear.Stats
                .Where(s => _upgradeController.UpgradeType.StatsTypes.Contains(s.StatInfo.Type));

            for (int i = 0; i < _changeFields.Length; i++)
            {
                int index = i;
                if (i < stats.Count())
                    _changeFields[index].EntityStat = stats.ElementAt(index);
                else
                    _changeFields[index].EntityStat = null;
            }
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