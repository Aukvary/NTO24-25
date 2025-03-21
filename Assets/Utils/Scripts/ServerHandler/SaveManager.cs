using NTO24.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        [SerializeField]
        private Image _errorWindow;

        [SerializeField]
        private Button _errorContinueButton;

        [SerializeField]
        private Image _differenceWindow;

        [SerializeField]
        private Button _serverContinue;

        [SerializeField]
        private Button _localContinue;

        [SerializeField]
        private TextMeshProUGUI _differenceField;


        private User _dateUser;
        private string _serverDate;
        private string _localDate;

        private string _currentID;

        public static InitializeFrom InitializeFrom { get; private set; } 

        private void Awake()
        {
            InitializeFrom = InitializeFrom.Server;
            _currentID = PlayerPrefs.GetString(nameof(User), null);

            InitializeContinueButton();
            InitializeInputField();
            StartCoroutine(InitializeDate());

            _errorContinueButton.onClick.AddListener(() =>
            {
                SetDate(false);
                SceneChanger.Instance.LoadScene((int)Scenes.Map);
            });
        }

        private void InitializeContinueButton()
        {
            if (string.IsNullOrEmpty(_currentID))
            {
                _continueButton.enabled = false;
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

                StartCoroutine(TryConnect(onSuccses: () =>
                {
                    if (_serverDate != _localDate)
                    {
                        _differenceWindow.gameObject.SetActive(true);

                        _differenceField.text = _differenceField.text.Replace("SDate", _serverDate);
                        _differenceField.text = _differenceField.text.Replace("LDate", _localDate);

                        _serverContinue.onClick.AddListener(() =>
                        {
                            SetDate();
                            SceneChanger.Instance.LoadScene((int)Scenes.Map);
                        });

                        _localContinue.onClick.AddListener(() =>
                        {
                            InitializeFrom = InitializeFrom.Local;
                            SetDate();
                            SceneChanger.Instance.LoadScene((int)Scenes.Map);
                        });
                    }
                    else
                        SceneChanger.Instance.LoadScene((int)Scenes.Map);
                }));
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

            _createGameButton.enabled = !string.IsNullOrEmpty(_saveNameField.text);
            _saveNameField.onValueChanged.AddListener(s =>
            {
                _createGameButton.enabled = !string.IsNullOrEmpty(s);
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

                StartCoroutine(TryConnect(onSuccses:() =>
                {
                    SetDate();
                    SceneChanger.Instance.LoadScene((int)Scenes.Map);
                }));
            });
        }

        private IEnumerator InitializeDate()
        {
            _localDate = PlayerPrefs.GetString("Date");

            yield return ServerHandler.InitializeUser(
                $"{_currentID}_Date",
                new Dictionary<string, string[]>() { { "Date", new string[] { _localDate } } },
                u =>
                {
                    if (u == null)
                        return;
                    _dateUser = u;
                    _serverDate = u["Date"][0];
                });
        }

        private IEnumerator TryConnect(UnityAction OnFailed = null, UnityAction onSuccses = null)
        {
            var connection = false;

            yield return ServerHandler.CheckConnection(c =>
            {
                connection = c;
                ServerHandler.HasConnection = c;
            });

            if (connection)
            {
                onSuccses?.Invoke();
                yield break;
            }
            OnFailed?.Invoke();
            _errorWindow.gameObject.SetActive(true);
        }

        private void SetDate(bool updateServer = true)
        {
            string strDate = DateTime.UtcNow.ToString("f");
            PlayerPrefs.SetString("Date", strDate);

            if (!updateServer || _dateUser == null)
                return;
            
            _dateUser["Date"] = new string[] { strDate };
        }
    }
}