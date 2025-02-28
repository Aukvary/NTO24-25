using UnityEngine;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class IconHUD : Drawable
    {
        [SerializeField]
        private Image _icon;

        private IIconable _entity;

        public IIconable Entity
        {
            get => _entity;

            set
            {
                _icon.enabled = value != null;
                _icon.sprite = value?.Icon;
            }
        }

        public Sprite Icon => _entity?.Icon;
    }
}