using DG.Tweening;
using UnityEngine;

namespace NTO24.UI
{
#pragma warning disable CS8524
    [CreateAssetMenu(fileName = "OutOfScreenEffect", menuName = "UI Effects", order = 51)]
    public class OutOfScreenEffect : Effect
    {
        private enum Directions
        {
            Right,
            Left,
            Up,
            Down
        }

        [SerializeField]
        private Directions _direction;

        [SerializeField]
        private float _animationDuration;

        private Tween _animation;

        private Vector2 _startPosition;

        public override RectTransform Transform
        {
            get => base.Transform;

            set
            {
                base.Transform = value;
                _startPosition = value.position;
            }
        }

        private void Awake()
        {
            _startPosition = Transform?.position ?? Vector3.zero;
        }

        public override Tween Hide()
        {
            _animation?.Complete();

            _animation = _direction switch
            {
                Directions.Left => Transform.DOMoveX(0, _animationDuration),
                Directions.Right => Transform.DOMoveX(Screen.width, _animationDuration),
                Directions.Up => Transform.DOMoveY(Screen.height, _animationDuration),
                Directions.Down => Transform.DOMoveY(0, _animationDuration),
            };

            return _animation;
        }


        public override Tween Show()
        {
            _animation?.Complete();

            _animation = _direction switch
            {
                Directions.Left or Directions.Right
                    => Transform.DOMoveX(_startPosition.x, _animationDuration),

                Directions.Up or Directions.Down
                    => Transform.DOMoveY(_startPosition.y, _animationDuration)
            };

            return _animation;
        }
    }
}