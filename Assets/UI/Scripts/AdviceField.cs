using TMPro;
using UnityEngine;

public class AdviceField : MonoBehaviour
{
    private TextMeshProUGUI _textField;

    private void Awake()
    {
        _textField = GetComponentInChildren<TextMeshProUGUI>();
        SetAdvice(@"выбери медведя, кликнув на него или нажав 1, 2, 3");
    }

    public void SetAdvice(string newAdvice)
    {
        _textField.text = newAdvice;
    }
}