using NTO24.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class TutorialController : MonoBehaviour, ISavableComponent
    {
        [Serializable]
        private class Advice
        {
            [field: SerializeField]
            public string Title { get; private set; }

            [field: SerializeField, TextArea]
            public string Text { get; private set; }
        }

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private List<Advice> _advices;

        [SerializeField]
        private float _timeToClose;

        [SerializeField]
        private UnityEvent _onCompleteEvent;

        private Advice _currentAdvice;

        private bool _finished;

        private EntitySelector _selector;
        private UpgradeController _upgradeController;

        public string Name => "Tutorial";
        public string[] Data => new string[] { _finished.ToString() };
        public UnityEvent OnDataChangeEvent { get; private set; } = new();

        public string AdviceTitle => _currentAdvice?.Title;
        public string AdviceText => _currentAdvice?.Text;

        private IEnumerator[] _conditions;

        public void ServerInitialize(IEnumerable<string> data)
        {
            _finished = bool.Parse(data.ElementAt(0));
        }

        public IEnumerator StartAdvicing(EntitySelector selector, UpgradeController upgradeController)
        {
            if (_finished)
            {
                gameObject.SetActive(false);
                yield break;
            }

            _conditions = new IEnumerator[]
            {
                AwaitCameraMove(),
                AwaitSelectPlayer(),
                AwaitPlayerMove(),
                AwaitExtract(),
                AwaitTaskQueue(),
                AwaitLayOut(),
                AwaitBuilt(),
                AwaitUpgrade()
            };

            _selector = selector;
            _upgradeController = upgradeController;

            for (int i = 0; i < _conditions.Length; i++)
            {
                _currentAdvice = _advices[i];

                _title.text = AdviceTitle;
                _text.text = AdviceText;

                yield return _conditions[i];
                _onCompleteEvent.Invoke();
            }
                
            _onCompleteEvent.Invoke();
            _currentAdvice = _advices.Last();
            _finished = true;
            OnDataChangeEvent.Invoke();
            StartCoroutine(AttackWarning());
        }

        private IEnumerator AwaitCameraMove()
        {
            bool mouseMove = false;
            bool keyboardMove = false;

            CameraController.Instance.OnMouseMove.AddListener(() => mouseMove = true);
            CameraController.Instance.OnKeyBoardMove.AddListener(() => keyboardMove = true);

            yield return new WaitUntil(() => (mouseMove && keyboardMove) || Input.GetKeyDown(KeyCode.E));
        }

        private IEnumerator AwaitSelectPlayer()
        {
            bool mouseSelect = false;
            bool hotKeySelect = false;
            bool repeatSelect = false;

            _selector.OnEntitySelecteEvent.AddListener(e => mouseSelect = true);
            _selector.OnHotKeySelect.AddListener(e => hotKeySelect = true);
            _selector.OnRepeatSelectEvent.AddListener(e => repeatSelect = true);

            yield return new WaitUntil(() => (mouseSelect && hotKeySelect && repeatSelect) || Input.GetKeyDown(KeyCode.E));
        }

        private IEnumerator AwaitPlayerMove()
        {
            var bears = Entity.Entities
                .Where(e => e is Bear).Select(b => b as IMovable);

            bool startMove = false;

            foreach (var bear in bears)
                bear.OnDestinationChangedEvent.AddListener(v => startMove = true);

            yield return new WaitUntil(() => startMove || Input.GetKeyDown(KeyCode.E));
        }

        private IEnumerator AwaitExtract()
        {
            var bears = Entity.Entities
                .Where(e => e is Bear).Select(b => b as IInventoriable);

            bool extract = false;

            foreach (var bear in bears)
                bear.OnItemsChangeEvent.AddListener(() => extract = true);

            yield return new WaitUntil(() => extract || Input.GetKeyDown(KeyCode.E));
        }

        private IEnumerator AwaitTaskQueue()
        {
            var bears = Entity.Entities
                .Where(e => e is Bear).Select(b => b as ITaskSolver);

            bool add = false;

            foreach (var bear in bears)
                bear.OnAddEvent.AddListener(t => add = true);

            yield return new WaitUntil(() => add || Input.GetKeyDown(KeyCode.E));
        }

        private IEnumerator AwaitLayOut()
        {
            var storage = Entity.Entities.First(e => e is Storage);

            bool layOut = false;

            (storage as IInventoriable).OnItemsChangeEvent
                .AddListener(() => layOut = true);

            yield return new WaitUntil(() => layOut || Input.GetKeyDown(KeyCode.E));
        }

        private IEnumerator AwaitBuilt()
        {
            var apires = Entity.Entities
                .Where(e => e is ConstructionObject)
                .Select(e => e as ConstructionObject);

            bool upgrade = false;

            foreach (var apire in apires)
                apire.OnBuiltEvent.AddListener(() => upgrade = true);

            yield return new WaitUntil(() => upgrade || Input.GetKeyDown(KeyCode.E));
        }

        private IEnumerator AwaitUpgrade()
        {
            bool upgrade = false;

            _upgradeController
                .OnUpgradeEvent.AddListener(() => upgrade = true);

            yield return new WaitUntil(() => upgrade || Input.GetKeyDown(KeyCode.E) );
        }

        private IEnumerator AttackWarning()
        {
            _title.text = AdviceTitle;
            _text.text = AdviceText;
            yield return new WaitForSeconds(_timeToClose);
            GetComponent<AnimatedUI>().Hide();
        }
    }
}