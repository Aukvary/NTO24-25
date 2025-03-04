using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class StatChangesField : Drawable
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _changesText;

        private EntityStat _entityStat;

        public EntityStat EntityStat
        {
            get => _entityStat;

            set
            {
                _entityStat?.RemoveOnLevelChangeAction(UpdateChanges);
                value?.AddOnLevelChangeAction(UpdateChanges);

                _entityStat = value;

                _icon.enabled = value != null;
                _changesText.enabled = value != null;

                if (value == null)
                    return;

                UpdateChanges();
            }
        }

        private void UpdateChanges()
        {
            _icon.enabled = EntityStat.CurrentLevel != EntityStat.MaxLevel;
            _changesText.enabled = EntityStat.CurrentLevel != EntityStat.MaxLevel;

            if (EntityStat.CurrentLevel == EntityStat.MaxLevel)
                return;

            _icon.sprite = EntityStat.StatInfo.Icon;

            _changesText.text = $"{EntityStat.StatValue} -> {EntityStat.NextStatValue}";
        }
    }
}