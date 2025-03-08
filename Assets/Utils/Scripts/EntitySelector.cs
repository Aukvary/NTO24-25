using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NTO24
{
    public class EntitySelector : MonoBehaviour
    {
        private UnityEvent<Entity> _onEntitySelecteEvent = new();
        private UnityEvent<Entity> _onRepeatSelectEvent = new(); 

        private EntryPoint _entryPoint;

        private Entity _selectedEntity;

        private bool UIRayCast => EventSystem.current.IsPointerOverGameObject();
        private Ray Direction => Camera.main.ScreenPointToRay(Input.mousePosition);

        private void Awake()
        {
            _entryPoint = GetComponent<EntryPoint>();
            AddSelectAction(e => {
                if (e == _selectedEntity)
                    _onRepeatSelectEvent.Invoke(e);

                _selectedEntity = e;
            });
        }

        private void Update()
        {
            MosueSelect();
            HotKeySelect();
        }

        private void MosueSelect() 
        {
            if (UIRayCast || !Input.GetKeyDown(KeyCode.Mouse0))
                return;

            if (!Physics.Raycast(Direction, out var hit))
                return;

            if (!hit.transform.TryGetComponent<Entity>(out var entity))
            {
                _onEntitySelecteEvent.Invoke(null);
                return;
            }

                _onEntitySelecteEvent.Invoke(entity);
        }

        private void HotKeySelect() 
        {
            KeyCode key = KeyCode.Alpha1;

            var bears = Entity.GetEntites<Bear>();

            for (int i = 0; i < bears.Count(); i++, key++)
                if (Input.GetKeyDown(key))
                    _onEntitySelecteEvent.Invoke(bears.ElementAt(i));
        }

        public void AddSelectAction(UnityAction<Entity> action)
            => _onEntitySelecteEvent.AddListener(action);

        public void RemoveSelectAction(UnityAction<Entity> action)
            => _onEntitySelecteEvent.RemoveListener(action);

        public void AddRepeatSelectAction(UnityAction<Entity> action)
            => _onRepeatSelectEvent.AddListener(action);

        public void RemoveRepeatSelectAction(UnityAction<Entity> action)
            => _onRepeatSelectEvent.RemoveListener(action);
    }
}