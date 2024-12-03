using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    private enum SoundType
    {
        Master,
        Music,
        Sound
    }
    [SerializeField]
    private Scrollbar _scrollbar;

    [SerializeField]
    private AudioMixerGroup _audioMixer;

    [SerializeField]
    private SoundType _soundType;

    private void Update()
    {
        _audioMixer.audioMixer.SetFloat("MusicVolume", -80 * _scrollbar.value);
    }
}