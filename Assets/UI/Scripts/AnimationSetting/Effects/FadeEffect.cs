using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace NTO24.UI
{
    [CreateAssetMenu(fileName = "ShadowingEffect", menuName = "UI Effects/Shadowing Effect", order = 51)]
    public class FadeEffect : UIEffect
    {
        [SerializeField]
        private float _appearDuration;

        [SerializeField]
        private float _appearDelay;

        [SerializeField]
        private float _fadeDuration;

        [SerializeField]
        private float _fadeDelay;

        private Image[] _images;

        private TMPro.TextMeshProUGUI[] _text;

        public override RectTransform Transform
        {
            get => base.Transform;

            set
            {
                base.Transform = value;
                _images = value.GetComponentsInChildren<Image>();
                _text = value.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            }
        }

        public override Tween Hide()
        {
            Sequence sequence = DOTween.Sequence();

            foreach (Image image in _images)
                sequence.Join(image.DOFade(0, _fadeDuration));
            
            foreach(TMPro.TextMeshProUGUI text in _text)
                sequence.Join(text.DOFade(0, _fadeDuration));

            sequence.SetDelay(_fadeDelay);

            foreach (Image iamge in _images)
                iamge.raycastTarget = false;
            return sequence;
        }

        public override Tween Show()
        {
            Sequence sequence = DOTween.Sequence();

            foreach (Image image in _images)
                sequence.Join(image.DOFade(1, _appearDuration));

            foreach (TMPro.TextMeshProUGUI text in _text)
                sequence.Join(text.DOFade(1, _appearDuration));

            foreach (Image iamge in _images)
                iamge.raycastTarget = true;

            sequence.SetDelay(_appearDelay);
            return sequence;
        }
    }
}