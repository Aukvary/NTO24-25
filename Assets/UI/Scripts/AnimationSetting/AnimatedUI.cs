using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24.UI
{
    public class AnimatedUI : Drawable
    {
        [SerializeField]
        private List<UIEffect> _effects;

        private Sequence _sequence;

        private bool _visible = true;

        protected override void Awake()
        {
            base.Awake();
            _effects.ForEach(e => e.Transform = transform);
        }

        public void Hide(TweenCallback onPlayCallBack = null, TweenCallback onCompleteCallBack = null)
        {
            if (!_visible)
                return;

            _visible = false;

            if (_sequence != null && _sequence.active)
                _sequence.Kill();

            _sequence = DOTween.Sequence();

            foreach (UIEffect effect in _effects)
                _sequence.Join(effect.Hide());

            _sequence.onPlay = onPlayCallBack;
            _sequence.onComplete = onCompleteCallBack;
            _sequence.Play();
        }

        public void Show(TweenCallback onPlayCallBack = null, TweenCallback onCompleteCallBack = null)
        {
            if (_visible)
                return;

            _visible = true;

            if (_sequence != null && _sequence.active)
                _sequence.Kill();

            _sequence = DOTween.Sequence();

            foreach (UIEffect effect in _effects)
                _sequence.Join(effect.Show());

            _sequence.onPlay = onPlayCallBack;
            _sequence.onComplete = onCompleteCallBack;
            _sequence.Play();
        }

        public void Complete()
        {
            if (_sequence != null && _sequence.active)
                _sequence?.Complete();
        }
    }
}