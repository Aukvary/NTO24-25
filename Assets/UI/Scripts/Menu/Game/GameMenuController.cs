using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _continueButtonObject;

    [SerializeField]
    private Color _cantContinue;

    private Color _canContinue;

    private Image[] _continueButtonRenderers;

    private Button _continueButton;

    private void Awake()
    {
        _continueButtonRenderers = _continueButtonObject.GetComponentsInChildren<Image>();
        _continueButton = _continueButtonObject.GetComponentInChildren<Button>();
        _canContinue = _continueButtonRenderers[0].color;
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(User.PlayerID))
        {
            foreach (var renderer in _continueButtonRenderers)
            {
                renderer.color = _cantContinue;
                _continueButton.enabled = false;
            }
        }
        else
        {
            foreach (var renderer in _continueButtonRenderers)
            {
                renderer.color = _canContinue;
                _continueButton.enabled = false;
            }
        }
    }
}