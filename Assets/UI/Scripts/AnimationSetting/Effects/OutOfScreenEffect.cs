using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NTO24.UI
{
#pragma warning disable CS8524
    [CreateAssetMenu(fileName = "OutOfScreenEffect", menuName = "UI Effects/Out Of Screen Effect", order = 51)]
    public class OutOfScreenEffect : UIEffect
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
        private float _appearDuration;

        [SerializeField]
        private float _disappearDuration;

        private Tween _animation;

        private Vector2 _startPosition;

        private Graphic[] _raycasters;

        public override RectTransform Transform
        {
            get => base.Transform;

            set
            {
                base.Transform = value;
                _raycasters = value.GetComponentsInChildren<Graphic>();
                _startPosition = value.position;
            }
        }

        private void Awake()
        {
            _startPosition = Transform?.position ?? Vector3.zero;
        }

        public override Tween Hide()
        {
            _animation = _direction switch
            {
                Directions.Left => Transform.DOMoveX(0, _disappearDuration),
                Directions.Right => Transform.DOMoveX(Screen.width, _disappearDuration),
                Directions.Up => Transform.DOMoveY(Screen.height, _disappearDuration),
                Directions.Down => Transform.DOMoveY(0, _disappearDuration),
            };

            foreach (var caster in _raycasters)
                caster.raycastTarget = false;

            return _animation;
        }


        public override Tween Show()
        {
            _animation = _direction switch
            {
                Directions.Left or Directions.Right
                    => Transform.DOMoveX(_startPosition.x, _appearDuration),

                Directions.Up or Directions.Down
                    => Transform.DOMoveY(_startPosition.y, _appearDuration)
            };

            foreach (var caster in _raycasters)
                caster.raycastTarget = true;

            return _animation;
        }
    }
}