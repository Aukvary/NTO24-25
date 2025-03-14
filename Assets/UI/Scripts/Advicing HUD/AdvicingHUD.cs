using TMPro;
using UnityEngine;

namespace NTO24.UI
{
    public class AdvicingHUD : Drawable
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private TutorialController _controller;

        public void Initialize(TutorialController controller)
        {
            _controller = controller;

            _controller.OnAdviceSetEvent.AddListener(UpdateText);
        }

        private void UpdateText()
        {
            _text.text = _controller.AdviceText ?? "";
        }
    }
}