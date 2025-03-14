using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat(_audioMixer.name, 1f);
        GetComponent<Scrollbar>().value = volume;
    }

    public void Set(float volume)
    {
        float vol = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        _audioMixer.SetFloat("MasterVolume", vol);
        PlayerPrefs.SetFloat(_audioMixer.name, volume);
    }
}