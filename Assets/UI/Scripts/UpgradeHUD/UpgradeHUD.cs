using System.Collections.Generic;
using UnityEngine;

namespace NTO24.UI
{
    public class UpgradeHUD : Drawable 
    {
        private AnimatedUI _uiAnimator;

        public void Initialize()
        {
            _uiAnimator = GetComponent<AnimatedUI>();
            _uiAnimator.Hide();
            _uiAnimator.Complete();
        }

        protected override void Update()
        {
            if (Input.GetKey(KeyCode.Tab))
                _uiAnimator.Show();
            else
                _uiAnimator.Hide();
        }
    }
}