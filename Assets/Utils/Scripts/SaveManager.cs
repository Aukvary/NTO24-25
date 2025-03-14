using NTO24.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NTO24
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField]
        private Button _continueButton;

        [SerializeField]
        private Button _createGameButton;

        [SerializeField]
        private TMP_InputField _saveNameField;

        private string _currentID;

        private void Awake()
        {
            _currentID = PlayerPrefs.GetString(nameof(User), null);

            InitializeContinueButton();
            InitializeInputField();
        }

        private void InitializeContinueButton()
        {
            if (string.IsNullOrEmpty(_currentID))
            {
                var image = _continueButton.GetComponent<Image>();

                Color color = image.color;
                color.a = 0.5f;
                image.color = color;

                var text = _continueButton.GetComponentInChildren<TextMeshProUGUI>();

                color = text.color;
                color.a = 0.5f;
                text.color = color;
            }

            _continueButton.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(_currentID))
                    return;

                SceneChanger.Instance.LoadScene((int)Scenes.SampleScene);
            });
        }

        private void InitializeInputField()
        {
            var image = _createGameButton.GetComponent<Image>();
            var text = _createGameButton.GetComponentInChildren<TextMeshProUGUI>();

            Color iamgeColor = image.color;
            iamgeColor.a = 0.5f;
            image.color = iamgeColor;


            var textColor = text.color;
            textColor.a = 0.5f;
            text.color = textColor;

            _saveNameField.onValueChanged.AddListener(s =>
            {
                if (string.IsNullOrEmpty(s))
                {
                    var iamgeColor = image.color;
                    iamgeColor.a = 0.5f;
                    image.color = iamgeColor;


                    var textColor = text.color;
                    textColor.a = 0.5f;
                    text.color = textColor;
                }
                else
                {
                    var iamgeColor = image.color;
                    iamgeColor.a = 1;
                    image.color = iamgeColor;


                    var textColor = text.color;
                    textColor.a = 1;
                    text.color = textColor;
                }
            });

            _createGameButton.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(_saveNameField.text))
                    return;

                if (_currentID != null)
                    StartCoroutine(ServerHandler.DeleteSave(_currentID));

                PlayerPrefs.SetString(nameof(User), _saveNameField.text);
                SceneChanger.Instance.LoadScene((int)Scenes.SampleScene);
            });
        }
    }
}