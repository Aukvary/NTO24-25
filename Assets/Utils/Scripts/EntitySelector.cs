using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NTO24
{
    public class EntitySelector : MonoBehaviour
    {
        private UnityEvent<Entity> _onEntitySelect = new();

        private bool UIRayCast => EventSystem.current.IsPointerOverGameObject();
        private Ray Direction => Camera.main.ScreenPointToRay(Input.mousePosition);

        private void Update()
        {
            if (/*UIRayCast ||*/ !Input.GetKeyDown(KeyCode.Mouse0))
                return;

            if (!Physics.Raycast(Direction, out var hit))
                return;

            if (!hit.transform.TryGetComponent<Entity>(out var entity))
            {
                _onEntitySelect.Invoke(null);
                return;
            }

                _onEntitySelect.Invoke(entity);
        }

        public void AddListner(UnityAction<Entity> action)
            => _onEntitySelect.AddListener(action);

        public void RemoveListner(UnityAction<Entity> action)
            => _onEntitySelect.RemoveListener(action);
    }
}