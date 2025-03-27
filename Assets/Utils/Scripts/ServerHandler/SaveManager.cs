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

        [SerializeField]
        private TMP_InputField _seedField;

        private User _dateUser;
        private string _serverDate;
        private string _localDate;

        private string _currentID;

        private int _serverSeed;

        public static InitializeFrom InitializeFrom { get; private set; }
        public static int Seed { get; private set; } = 994320754;

        private void Start()
        {
            StartCoroutine(Initialize());
        }

        public IEnumerator Initialize()
        {
            InitializeFrom = InitializeFrom.Server;
            _currentID = PlayerPrefs.GetString(nameof(User), null);
            yield return InitializeDate();

            InitializeContinueButton();
            InitializeInputField();

            _errorContinueButton.onClick.AddListener(() =>
            {

                Seed = int.Parse(_seedField.text);
                print(Seed);
                PlayerPrefs.SetInt("seed", Seed);
                SetDate(false);
                SceneChanger.Instance.LoadScene(GetMap());
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
                            SceneChanger.Instance.LoadScene(GetMap());
                        });

                        _localContinue.onClick.AddListener(() =>
                        {
                            InitializeFrom = InitializeFrom.Local;
                            SetDate();
                            SceneChanger.Instance.LoadScene(GetMap());
                        });
                    }
                    else
                        SceneChanger.Instance.LoadScene(GetMap());
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

                    Seed = int.Parse(_seedField.text);
                    print(Seed);
                    PlayerPrefs.SetInt("seed", Seed);

                    SetDate();
                    SceneChanger.Instance.LoadScene(GetMap());
                }));
            });
        }

        private IEnumerator InitializeDate()
        {
            _localDate = PlayerPrefs.GetString("Date");

            yield return ServerHandler.InitializeUser(
                $"{_currentID}_Date",
                new Dictionary<string, string[]>() { { "Date", new string[] { _localDate } },
                    { "Seed", new string[] { PlayerPrefs.GetInt("seed", 0).ToString() } } },
                u =>
                {
                    if (u == null)
                        return;
                    _dateUser = u;
                    _serverDate = u["Date"][0];
                    _serverSeed = int.Parse(u["seed"][0]);
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
                if (PlayerPrefs.GetInt("seed", 0) == 0)
                    Seed = _serverSeed;
                yield break;
            }
            OnFailed?.Invoke();
            _errorWindow.gameObject.SetActive(true);
        }

        private void SetDate(bool updateServer = true)
        {
            string strDate = DateTime.UtcNow.ToString("f");
            PlayerPrefs.SetString("Date", strDate);
            Seed = PlayerPrefs.GetInt("seed");

            if (!updateServer || _dateUser == null)
                return;
            
            _dateUser["Date"] = new string[] { strDate };
            _dateUser["seed"] = new string[] { PlayerPrefs.GetInt("seed").ToString() };
        }

        private int GetMap()
        {
            return ((int)Scenes.Island1) + PlayerPrefs.GetInt("seed") % 3;
        }
    }
}