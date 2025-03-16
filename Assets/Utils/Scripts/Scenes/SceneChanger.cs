using NTO24.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NTO24
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField]
        private AnimatedUI _loadScreen;

        public static SceneChanger Instance { get; private set; }

        public void LoadScene(int index)
        {
            _loadScreen.Show(onCompleteCallBack: () => StartCoroutine(Load(index)));
        }

        private IEnumerator Load(int index)
        {
            yield return SceneManager.LoadSceneAsync(index);
            _loadScreen.Hide(onCompleteCallBack: () =>
            {
                Destroy(transform.parent.gameObject);
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