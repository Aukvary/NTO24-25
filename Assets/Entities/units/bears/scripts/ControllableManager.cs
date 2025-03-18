using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NTO24
{
    public class ControllableManager : MonoBehaviour
    {
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
    }
}
