using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    private enum SoundType
    {
        Music,
        Sound
    }
    [SerializeField]
    private Scrollbar _scrollbar;

    [SerializeField]
    private AudioMixerGroup _audioMixer;

    [SerializeField]
    private SoundType _soundType;

    private void Awake()
    {
        var name = _soundType == SoundType.Music ? "MusicVolume" : "SoundVolume";
        _audioMixer.audioMixer.SetFloat(name, PlayerPrefs.GetFloat(name, 0));
        _scrollbar.value = PlayerPrefs.GetFloat(name) / -80;
    }

    public void Set()
    {
        var name = _soundType == SoundType.Music ? "MusicVolume" : "SoundVolume";
        _audioMixer.audioMixer.SetFloat(name , -80 * _scrollbar.value);

        PlayerPrefs.SetFloat(name, -80 * _scrollbar.value);

        PlayerPrefs.Save();
    }
}