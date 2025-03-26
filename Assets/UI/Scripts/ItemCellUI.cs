using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class ItemCellUI : MonoBehaviour
    {
        [SerializeField]
        private Image _resourceImage;

        [SerializeField]
        private TextMeshProUGUI _countText;

        public Pair<Resource, int>? Source
        {
            set
            {
                _resourceImage.enabled = value?.Value1 != null;
                if (_countText == null)
                    print(transform.parent.parent.name);
                _countText.enabled = value?.Value1 != null;
                if (value?.Value1 == null)
                    return;

                _resourceImage.sprite = value.Value.Value1.Sprite;
                _countText.text = value.Value.Value2.ToString();

            }
        }
    }
}