using System.Collections;
using UnityEngine;

namespace NTO24.UI
{
    public class FinalSceneUI : Drawable
    {
        private AnimatedUI _animatedUI;

        protected override void Start()
        {
            _animatedUI = GetComponentInChildren<AnimatedUI>();

            _animatedUI.Hide();
            _animatedUI.Complete();
            _animatedUI.Show();
        }

        protected override void Update()
        {
            if (Input.anyKeyDown)
                SceneChanger.Instance.LoadScene((int)Scenes.MainMenu);
        }
    }
}