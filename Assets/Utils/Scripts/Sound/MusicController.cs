using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace NTO24
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer _musicMixer;

        [SerializeField]
        private AudioSource _calmMusic;

        [SerializeField]
        private AudioSource _fightMusic;

        [SerializeField]
        private float _changeSpeed;

        private int _beeCount = 0;

        private Coroutine _increaseCoroutine;
        private Coroutine _decreaseCoroutine;

        private void Awake()
        {
            Entity.OnAddEntity.AddListener(e =>
            {
                if (e is not Bee)
                    return;

                _beeCount++;

                if (_beeCount != 1)
                    return;

                if (_increaseCoroutine != null)
                {
                    StopCoroutine(_increaseCoroutine);
                    StopCoroutine(_decreaseCoroutine);
                }


                _increaseCoroutine = StartCoroutine(Set("Fight", true));
                _decreaseCoroutine = StartCoroutine(Set("Calm", false));
            });

            Entity.OnRemoveEntity.AddListener(e =>
            {
                if (e is not Bee)
                    return;

                _beeCount--;

                if (_beeCount != 0)
                    return;

                if (_increaseCoroutine != null)
                {
                    StopCoroutine(_increaseCoroutine);
                    StopCoroutine(_decreaseCoroutine);
                }


                _increaseCoroutine = StartCoroutine(Set("Calm", true));
                _decreaseCoroutine = StartCoroutine(Set("Fight", false));
            });
        }

        private IEnumerator Set(string name, bool increase)
        {
            if (name == "Fight")
                _fightMusic.Play();
            else
                _calmMusic.Play();

                _musicMixer.GetFloat(name, out var current);

            var volume = increase ? 1f : 0.0001f;

            volume = Mathf.Log10(volume) * 20;

            while (current != volume)
            {
                current = Mathf.Lerp(current, volume, Time.deltaTime * _changeSpeed);
                _musicMixer.SetFloat(name, current);
                yield return null;

            }
        }
    }
}