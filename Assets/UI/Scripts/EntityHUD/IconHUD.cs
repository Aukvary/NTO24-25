using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class IconHUD : Drawable
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private float _animationSpeed;

        private Sprite[] _frames;

        private Coroutine _frameChanger;

        private IIconable _entity;

        public IIconable Entity
        {
            get => _entity;

            set
            {
                _entity = value;
                _icon.enabled = value != null;
                if (_frameChanger != null)
                    StopCoroutine(_frameChanger);
                if (value == null)
                    return;
                _frames = value.Icon;
                _frameChanger = StartCoroutine(Change());
            }
        }

        public Sprite[] Icon
        {
            get => _frames;

            set
            {
                _icon.enabled = value != null;
                if (_frameChanger != null)
                    StopCoroutine(_frameChanger);
                if (value == null)
                    return;
                _frames = value;
                _frameChanger = StartCoroutine(Change());
            }
        }

        private IEnumerator Change()
        {
            if (_animationSpeed == 0)
            {
                _icon.sprite = _frames[0];
                yield break;
            }
            int i = 0;
            while (true)
            {
                _icon.sprite = _frames[i];
                yield return new WaitForSeconds(_animationSpeed);
                i = (i + 1) % _frames.Length;
            }
        }
    }
}