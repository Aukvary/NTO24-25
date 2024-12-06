using System.Linq;
using TMPro;
using UnityEngine;

public class GameEnder : MonoBehaviour
{
    [SerializeField]
    private float _time;

    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private float _appearSpeed;

    private bool _canEnd;

    private Color _color;

    private void Start()
    {
        StartCoroutine(StartEnd());
        _text.color = new(_text.color.r, _text.color.g, _text.color.b, 0);
    }

    private System.Collections.IEnumerator StartEnd()
    {
        yield return new WaitForSeconds(_time);
        _canEnd = true;
    }

    private void Update()
    {
        if (_canEnd)
            _text.color = Color.Lerp(_text.color, Color.white, Time.deltaTime * _appearSpeed);

        if (!_canEnd || !Input.anyKeyDown)
            return;

        PlayerPrefs.SetString(nameof(User), null);

        new SceneChanger().ExitToMenu();
    }
}