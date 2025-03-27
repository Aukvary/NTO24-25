using NTO24.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NTO24
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField]
        private AnimatedUI _loadScreen;

        public static SceneChanger Instance { get; private set; }

        public void LoadScene(
            int index,
            Func<IEnumerator> PreLoadCallBack = null,
            Func<IEnumerator> PostLoadCallBack = null)
        {
            _loadScreen.Show(onCompleteCallBack:
                    () => StartCoroutine(Load(index, PreLoadCallBack, PostLoadCallBack))
                );
        }

        public void LoadScene(int index)
        {
            _loadScreen.Show(onCompleteCallBack:
                    () => StartCoroutine(Load(index))
                );
        }

        private IEnumerator Load(
            int index,
            Func<IEnumerator> PreLoadCallBack = null,
            Func<IEnumerator> PostLoadCallBack = null)
        {
            yield return PreLoadCallBack?.Invoke();
            yield return SceneManager.LoadSceneAsync(index);
            _loadScreen.Hide(onCompleteCallBack: () =>
            {
                Destroy(transform.parent.gameObject);
                PostLoadCallBack?.Invoke();
            });
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        private void Start()
        {
            _loadScreen.Hide();
            _loadScreen.Complete();
        }
    }
}