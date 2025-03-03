using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace NTO24.UI
{
    public class UpgradeTypeButton : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Color _unselectedColor;

        [SerializeField]
        private Color _selectedColor;

        private Button _button;
        private Image _image;

        private UpgradeType _upgradeType;

        public UpgradeType UpgradeType
        {
            get => _upgradeType;

            set
            {
                _upgradeType = value;
                _icon.sprite = value.Icon;
            }
        }

        public ButtonClickedEvent OnClick => _button.onClick;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        }

        public UpgradeType IsSelected(bool select)
        {
            _image.color = select ? _selectedColor : _unselectedColor;
            return UpgradeType;
        }
    }
}