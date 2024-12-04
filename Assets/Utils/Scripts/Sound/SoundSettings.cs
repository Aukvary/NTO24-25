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

    private void Update()
    {
        
    }

    public void Set()
    {
        _audioMixer.audioMixer.SetFloat(_soundType == SoundType.Music ? "MusicVolume" : "SoundVolume"
            , -80 * _scrollbar.value);
    }
}