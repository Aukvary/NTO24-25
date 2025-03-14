using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NTO24
{
    public class TutorialController : MonoBehaviour, ISavableComponent
    {
        [Serializable]
        class Advice
        {
            [field: SerializeField, TextArea]
            public string Text { get; private set; }
        }

        [SerializeField]
        private List<Advice> _advices;

        [field: SerializeField]
        public UnityEvent OnTutorialEndEvent { get; private set; }
        public UnityEvent OnAdviceSetEvent { get; private set; } = new();

        private Advice _currentAdvice;

        private int _step = 0;

        private EntitySelector _selector;
        private UpgradeController _upgradeController;

        public string Name => "Tutorial";
        public string[] Data => new string[] { _step.ToString() };
        public UnityEvent OnDataChangeEvent { get; private set; } = new();

        public string AdviceText => _currentAdvice?.Text;

        private IEnumerator[] _conditions;

        public void ServerInitialize(IEnumerable<string> data)
        {
            _step = int.Parse(data.ElementAt(0));
        }

        public IEnumerator StartAdvicing(EntitySelector selector, UpgradeController upgradeController)
        {
            _conditions = new IEnumerator[]
            {
                AwaitCameraMove(),
                AwaitPlayerMove(),
                AwaitExtract(),
                AwaitLayOut(),

            };

            _selector = selector;
            _upgradeController = upgradeController;

            for (; _step < _advices.Count; _step++)
            {
                _currentAdvice = _advices[_step];
                OnAdviceSetEvent.Invoke();

                yield return _conditions[_step];
            }
            _currentAdvice = null;
            OnTutorialEndEvent.Invoke();
        }

        private IEnumerator AwaitCameraMove()
        {
            bool mouseMove = false;
            bool keyboardMove = false;

            CameraController.Instance.OnMouseMove.AddListener(() => mouseMove = true);
            CameraController.Instance.OnKeyBoardMove.AddListener(() => keyboardMove = true);

            yield return new WaitUntil(() => mouseMove && keyboardMove);
        }

        private IEnumerator AwaitSelectPlayer()
        {
            bool mouseSelect = false;
            bool hotKeySelect = false;
            bool repeatSelect = false;

            _selector.OnEntitySelecteEvent.AddListener(e => mouseSelect = true);
            _selector.OnHotKeySelect.AddListener(e => hotKeySelect = true);
            _selector.OnRepeatSelectEvent.AddListener(e => repeatSelect = true);

            yield return new WaitUntil(() => mouseSelect && hotKeySelect && repeatSelect);
        }

        private IEnumerator AwaitPlayerMove()
        {
            var bears = Entity.Entities
                .Where(e => e is Bear).Select(b => b as IMovable);

            bool startMove = false;

            foreach (var bear in bears)
                bear.OnDestinationChangedEvent.AddListener(v => startMove = true);

            yield return new WaitUntil(() => startMove);
        }

        private IEnumerator AwaitExtract()
        {
            var bears = Entity.Entities
                .Where(e => e is Bear).Select(b => b as IInventoriable);

            bool extract = false;

            foreach (var bear in bears)
                bear.OnItemsChangeEvent.AddListener(() => extract = true);

            yield return new WaitUntil(() => extract);
        }

        private IEnumerator AwaitLayOut()
        {
            var storage = Entity.Entities.First(e => e is Storage);

            bool layOut = false;

            (storage as IInventoriable).OnItemsChangeEvent
                .AddListener(() => layOut = true);

            yield return new WaitUntil(() => layOut);
        }

        private IEnumerator AwaitBuilt()
        {
            var apires = Entity.Entities
                .Where(e => e is ConstructionObject)
                .Select(e => e as ConstructionObject);

            bool upgrade = false;

            foreach (var apire in apires)
                apire.OnBuiltEvent.AddListener(() => upgrade = false);

            yield return new WaitUntil(() => upgrade);
        }

        private IEnumerator AwaitUpgrade()
        {
            bool upgrade = false;

            _upgradeController
                .OnUpgradeEvent.AddListener(() => upgrade = true);

            yield return new WaitUntil(() => upgrade);
        }
    }
}