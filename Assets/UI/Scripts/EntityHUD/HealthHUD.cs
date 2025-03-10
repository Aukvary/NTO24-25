using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class HealthHUD : Drawable
    {
        [SerializeField]
        private TextMeshProUGUI _healthText;

        [SerializeField]
        private TextMeshProUGUI _regenerationText;

        [SerializeField]
        private Image _healthBar;

        [SerializeField]
        private Color _minHealthColor;

        [SerializeField]
        private Color _maxHealthColor;

        [SerializeField]
        private Color _minBackColor;

        [SerializeField]
        private Color _maxBackColor;

        private Image _back;

        private UnityAction<Entity, HealthChangeType> _updateEntity;

        private IHealthable _entity;

        private Vector2 _minAnchor;
        private Vector2 _maxAnchor;

        public IHealthable Entity
        {
            get => _entity;

            set
            {
                _entity?.OnUpgadeEvent.RemoveListener(UpdateEntity);
                _entity?.OnHealthChangeEvent.RemoveListener(_updateEntity);

                value?.OnUpgadeEvent.AddListener(UpdateEntity);
                value?.OnHealthChangeEvent.AddListener(_updateEntity);

                _entity = value;

                _healthText.enabled = value != null;
                _regenerationText.enabled = value != null;
                _healthBar.gameObject.SetActive(value != null);

                if (Entity != null)
                    UpdateEntity();
            }
        }

        protected override void Awake()
        {
            _back = GetComponent<Image>();

            _updateEntity = (a, b) => UpdateEntity();
            _minAnchor = _healthBar.rectTransform.anchorMin;
            _maxAnchor = _healthBar.rectTransform.anchorMax;
        }

        private void UpdateEntity()
        {
            _healthText.text = $"{(int)Entity.Health} / {Entity.MaxHealth}";
            _regenerationText.text = $"+{Entity.Regeneration.ToString()}";
            _healthBar.rectTransform.anchorMax = new(
                    Mathf.Lerp(_minAnchor.x, _maxAnchor.x, Entity.Health / Entity.MaxHealth), 
                    _maxAnchor.y
            );
            _healthBar.color = Color.Lerp(_minHealthColor, _maxHealthColor, Entity.Health / Entity.MaxHealth);
            _back.color = Color.Lerp(_minBackColor, _maxBackColor, Entity.Health / Entity.MaxHealth);
        }
    }
}