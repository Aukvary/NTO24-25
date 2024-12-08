using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameSetter : MonoBehaviour
{
    [SerializeField]
    private GameObject _newGameButton;

    [SerializeField]
    private Color _cantStartColor;

    private Color _canStartColor;

    private TMP_InputField _textField;

    private Image[] _newGameRenderers;
    private Button _canStartButton;

    private void Awake()
    {
        _textField = GetComponentInChildren<TMP_InputField>();
        _newGameRenderers = _newGameButton.GetComponentsInChildren<Image>();
        _canStartButton = _newGameButton.GetComponentInChildren<Button>();

        _canStartColor = _newGameRenderers[0].color;

        _textField.onValueChanged.AddListener(s =>
        {
            _canStartButton.enabled = !string.IsNullOrEmpty(s);
            foreach (var render in _newGameRenderers)
                render.color = string.IsNullOrEmpty(s) ? _cantStartColor : _canStartColor;
        });
    }

    private void OnEnable()
    {
        _canStartButton.enabled = !string.IsNullOrEmpty(_textField.text);
        foreach (var render in _newGameRenderers)
            render.color = string.IsNullOrEmpty(_textField.text) ? _cantStartColor : _canStartColor;
    }
}