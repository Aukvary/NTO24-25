using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace NTO24
{
    public class SoundInitializer : MonoBehaviour
    {
        [SerializeField]
        private List<AudioMixer> _mixers;

        private void Start()
        {
            foreach (var mixer in _mixers)
            {
                float volume = PlayerPrefs.GetFloat(mixer.name);
                volume = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
                mixer.SetFloat("MasterVolume", volume);
            }
        }
    }
}