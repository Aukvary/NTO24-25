using DG.Tweening;
using UnityEngine;

namespace NTO24.UI
{
    public abstract class Effect : ScriptableObject
    {
        public virtual RectTransform Transform { get; set; }

        public abstract Tween Hide();

        public abstract Tween Show();
    }
}