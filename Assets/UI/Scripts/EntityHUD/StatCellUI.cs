using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace NTO24.UI
{
    public class StatCellUI : Drawable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image _discriptionField;

        [SerializeField]
        private TextMeshProUGUI _valueText;

        [SerializeField]
        private TextMeshProUGUI _titleText;

        [SerializeField]
        private TextMeshProUGUI _descriptionText;

        [SerializeField]
        private Image _icon;

        private Image _image;

        private EntityStat _stat;

        public EntityStat Stat
        {
            get => _stat;

            set
            {
                _stat?.RemoveOnLevelChangeAction(UpdateStats);
                value?.AddOnLevelChangeAction(UpdateStats);

                _stat = value;

                _image.enabled = _stat != null;
                _image.enabled = _stat != null;
                if (_stat == null)
                    return;

                UpdateStats();           
            }
        }

        private bool Active
        {
            get => _discriptionField.gameObject.activeSelf;

            set => _discriptionField.gameObject.SetActive(value);
        }

        protected override void Awake()
        {
            base.Awake();
            _image = GetComponent<Image>();
            _discriptionField.gameObject.SetActive(false);
        }

        private void UpdateStats()
        {
            _valueText.text = Stat.StatValue.ToString();

            _titleText.text = Stat.StatInfo.Title;
            _descriptionText.text = Stat.StatInfo.Description;

            _icon.sprite = Stat.StatInfo.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_stat == null)
                return;
            Active = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_stat == null)
                return;

            Active = false;
        }
    }
}