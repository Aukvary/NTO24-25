using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class GameMenu : Drawable
    {
        [SerializeField]
        private Image _animatedUI;

        [SerializeField]
        private Image _settingsWindow;

        private static List<Func<IEnumerator>> _onExitEvents = new();

        private bool _active = false;

        protected override void Start()
        {
            _animatedUI.gameObject.SetActive(false);
        }

        protected override void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
                return;

            _settingsWindow.gameObject.SetActive(false);
            
            _active = !_active;

            Time.timeScale = _active ? 0f : 1f;

            _animatedUI.gameObject.SetActive(_active);
        }

        public void Continue()
        {
            _active = false;
            Time.timeScale = 1f;
            _animatedUI.gameObject.SetActive(false);
        }

        public void Exit()
        {
            SceneChanger.Instance.LoadScene((int)Scenes.MainMenu,
                PreLoadCallBack: OnExitInvoke);
        }

        public static void AddOnExitAction(Func<IEnumerator> method)
            => _onExitEvents.Add(method);

        private IEnumerator OnExitInvoke()
        {
            foreach (var method in _onExitEvents)
                yield return method.Invoke();
        }

        public void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}