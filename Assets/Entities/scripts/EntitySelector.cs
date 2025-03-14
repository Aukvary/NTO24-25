using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NTO24
{
    public class EntitySelector : MonoBehaviour
    {
        public UnityEvent<Entity> OnEntitySelecteEvent { get; private set; } = new();
        public UnityEvent<Entity> OnHotKeySelect { get; private set; } = new();
        public UnityEvent<Entity> OnRepeatSelectEvent { get; private set; } = new();

        private IEnumerable<Bear> _bears;

        private Entity _selectedEntity;

        private bool UIRayCast => EventSystem.current.IsPointerOverGameObject();
        private Ray Direction => Camera.main.ScreenPointToRay(Input.mousePosition);

        private void Awake()
        {
            OnEntitySelecteEvent.AddListener(e => {
                if (e == _selectedEntity)
                    OnRepeatSelectEvent.Invoke(e);

                _selectedEntity = e;
            });
        }

        private void Start()
        {
            _bears = Entity.GetEntites<Bear>();
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

            var hits = Physics.RaycastAll(Direction);

            foreach(var hit in hits)
            {
                if (!hit.transform.TryGetComponent<Entity>(out var entity))
                    continue;
                
                OnEntitySelecteEvent.Invoke(entity);
                return;
            }
            OnEntitySelecteEvent.Invoke(null);

        }

        private void HotKeySelect() 
        {
            KeyCode key = KeyCode.Alpha1;

            for (int i = 0; i < _bears.Count(); i++, key++)
                if (Input.GetKeyDown(key))
                {
                    OnEntitySelecteEvent.Invoke(_bears.ElementAt(i));
                    OnHotKeySelect.Invoke(_bears.ElementAt(i));
                }
        }
    }
}