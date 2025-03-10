using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NTO24
{
    public class ControllableManager : MonoBehaviour
    {
        [Header("Unit Selection")]
        [SerializeField]
        private Color _selectingAreaColor;

        [SerializeField]
        private float _areaAlpha;

        [SerializeField]
        private float _areaBorderThickness;

        private List<Unit> _selectedUnits;

        private Vector3 _startMousePosition;
        private Vector3 _endMousePosition;

        public IEnumerable<Unit> SelectedUnits => _selectedUnits;

        private bool UIRayCast => EventSystem.current.IsPointerOverGameObject();
        private Ray _direction => Camera.main.ScreenPointToRay(Input.mousePosition);

        public void Initialize(EntitySelector selector)
        {
            selector.OnEntitySelecteEvent.AddListener(SelectAloneUnit);

            _selectedUnits = new();
        }

        private void Update()
        {
            SetTask();
            SelectUnitGroup();

            if (Input.GetKeyDown(KeyCode.H))
            {
                foreach (var unit in SelectedUnits)
                    (unit as IHealthable).Damage(10);
            }
        }

        private void SetTask()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse1) || 
                Input.GetKey(KeyCode.LeftAlt) ||
                !SelectedUnits.Any()|| 
                UIRayCast)
                return;

            if (!Physics.Raycast(_direction, out var actionHit))
                return;

            var units = Input.GetKey(KeyCode.LeftControl) ?
                Entity.GetEntites<Unit>().Where(u => u is ITaskSolver) : SelectedUnits;

            if (Input.GetKey(KeyCode.LeftShift))
                foreach (var unit in units)
                    (unit as ITaskSolver).AddTask(CreateTasks(unit, actionHit));
            else
                foreach (var unit in units)
                    (unit as ITaskSolver).SetTask(CreateTasks(unit, actionHit));
        }

        private IEnumerable<IUnitTask> CreateTasks(Unit unit, RaycastHit hit)
        {
            if (!hit.transform.TryGetComponent<Entity>(out var entity))
                return new IUnitTask[] { new MoveToVectorTask(unit, hit.point) };

            switch (entity)
            {
                case IInteractable interactable:
                    return new IUnitTask[]
                    {
                        new InteractTask(unit as IInteractor, interactable)
                    };
                case IHealthable e when e.EntityReference.EntityType != unit.EntityType && e.EntityReference != unit:
                    return new IUnitTask[]
                    {
                        new AttackTask(unit as IAttacker, e)
                    };
                default:
                    return new IUnitTask[0];
            }
        }


        private void SelectAloneUnit(Entity entity)
        {
            if (entity is not IControllable unit)
                return;

            _selectedUnits.Clear();

            _selectedUnits.Add(unit as Unit);
        }
    
        private void SelectUnitGroup()
        {
            if (_startMousePosition == _endMousePosition)
                return;
            Vector3 v1 = Camera.main.ScreenToViewportPoint(_startMousePosition);
            Vector3 v2 = Camera.main.ScreenToViewportPoint(_endMousePosition);

            Vector3 minOffset = Vector3.Min(v1, v2);
            Vector3 maxOffset = Vector3.Max(v1, v2);

            minOffset.z = Camera.main.nearClipPlane;
            maxOffset.z = Camera.main.farClipPlane;

            Bounds bounds = new Bounds();
            bounds.SetMinMax(minOffset, maxOffset);

            foreach (var unit in Entity.GetEntites<IControllable>())
            {
                if (!bounds.Contains(Camera.main.WorldToViewportPoint(unit.EntityReference.transform.position)))
                    continue;
                _selectedUnits.Add(unit.EntityReference as Unit);
            }
        }

        private void OnGUI()
        {
            if (_startMousePosition != _endMousePosition && !Input.GetKey(KeyCode.Mouse0))
            {
                _startMousePosition = _endMousePosition = Vector3.zero;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _startMousePosition = Input.mousePosition;
                _startMousePosition.y = Screen.height - _startMousePosition.y;

            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                _endMousePosition = Input.mousePosition;
                _endMousePosition.y = Screen.height - _endMousePosition.y;
            }

            var topLeft = Vector3.Min(_startMousePosition, _endMousePosition);
            var bottomRight = Vector3.Max(_startMousePosition, _endMousePosition);

            var rect = Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);


            Color areaColor = _selectingAreaColor;
            areaColor.a = _areaAlpha;
            UIUtils.DrawSelectArea(rect, areaColor, _selectingAreaColor, _areaBorderThickness);
        }
    }
}
