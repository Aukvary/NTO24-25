using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class StatCellUI : Drawable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private AnimatedUI _discriptionField;

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
                _icon.enabled = _stat != null;
                Active &= _stat != null;

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
        }

        protected override void Start()
        {
            Active = false;
            _discriptionField.Hide();
            _discriptionField.Complete();
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
            //_discriptionField.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_stat == null)
                return;
            Active = false;
            //_discriptionField.Hide();
        }
    }
}