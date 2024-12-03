using System.Linq;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _calmMusic;

    [SerializeField]
    private AudioSource _fightMusic;

    [SerializeField]
    private float _changeSpeed;

    private BearActivityManager _activityManager;

    private bool _fightNow = false;

    private bool _fight => _activityManager.AllUnits.Any(u => u != null && u.IsBee);

    private void Awake()
        => _activityManager = GetComponent<BearActivityManager>();


    private void Update()
    {
        if (_fight && !_fightNow)
        {
            StopCoroutine(Swap());
            StartCoroutine(Swap());
            _fightNow = true;
        }
        else if (!_fight && _fightNow)
        {
            StopCoroutine(Swap());
            StartCoroutine(Swap());
            _fightNow = false;
        }
    }

    private System.Collections.IEnumerator Swap()
    {
        float calm = _fightMusic.volume;
        float fight = _calmMusic.volume;

        if (calm > 0) _calmMusic.Play();
        else _fightMusic.Play();

        while (_calmMusic.volume != calm || _fightMusic.volume != fight)
        {
            _calmMusic.volume = Mathf.Lerp(_calmMusic.volume, calm, Time.deltaTime * _changeSpeed);
            _fightMusic.volume = Mathf.Lerp(_fightMusic.volume, fight, Time.deltaTime * _changeSpeed);

            

            yield return null;
        }

        if (calm > 0) _fightMusic.Stop(); 
        else _calmMusic.Stop();
    }
}