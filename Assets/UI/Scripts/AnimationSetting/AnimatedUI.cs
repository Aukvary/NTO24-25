using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24.UI
{
    public class AnimatedUI : Drawable
    {
        [SerializeField]
        private List<Effect> _effects;

        private Sequence _sequence;

        protected override void Awake()
        {
            base.Awake();
            _effects.ForEach(e => e.Transform = transform);
        }

        public void Hide(TweenCallback onPlayCallBack = null, TweenCallback onCompleteCallBack = null)
        {
            if (_sequence != null && _sequence.active)
                _sequence.Complete();

            _sequence = DOTween.Sequence().Append(_effects[0].Hide());

            foreach (Effect effect in _effects.Skip(1))
                _sequence = _sequence.Join(effect.Hide());
            
            _sequence.onPlay = onPlayCallBack;
            _sequence.onComplete = onCompleteCallBack;
        }

        public void Show(TweenCallback onPlayCallBack = null, TweenCallback onCompleteCallBack = null)
        {
            if (_sequence != null && _sequence.active)
                _sequence.Complete();

            _sequence = DOTween.Sequence().Append(_effects[0].Show());

            foreach (Effect effect in _effects.Skip(1))
                _sequence = _sequence.Join(effect.Show());

            _sequence.onPlay = onPlayCallBack;
            _sequence.onComplete = onCompleteCallBack;
        }

        public void Complete()
        {
            if (_sequence != null && _sequence.active)
                _sequence?.Complete();
        }
    }
}