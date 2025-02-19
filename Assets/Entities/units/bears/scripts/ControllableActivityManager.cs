using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllableActivityManager : MonoBehaviour
{
    [Header("Unit Selection")]
    [SerializeField]
    private Color _selectingAreaColor;

    [SerializeField]
    private float _areaAlpha;

    [SerializeField]
    private float _areaBorderThickness;

    private EntryPoint _entryPoint;

    private List<Unit> _selectedUnits;

    private Vector3 _startMousePosition;
    private Vector3 _endMousePosition;

    public IEnumerable<Unit> SelectedUnits => _selectedUnits;

    private bool UIRayCast => EventSystem.current.IsPointerOverGameObject();
    private Ray direction => Camera.main.ScreenPointToRay(Input.mousePosition);

    public void Initialize(EntryPoint entryPoint)
    {
        _entryPoint = entryPoint;
    }

    private void Update()
    {
        SetTask();
        SelectUnitGroup();
        SelectAloneUnit();
    }

    private void SetTask()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.LeftAlt) || UIRayCast)
            return;

        if (!Physics.Raycast(direction, out var actionHit))
            return;

        var units = Input.GetKey(KeyCode.LeftControl) ?
            _entryPoint.Units.Where(u => u is ITaskSolver) : SelectedUnits;

        if (Input.GetKey(KeyCode.LeftShift))
            foreach (var unit in units)
                unit.AddTask(CreateTask(unit, actionHit));
        else
            foreach (var unit in units)
                unit.SetTask(CreateTask(unit, actionHit));
    }

    private IEnumerable<IUnitTask> CreateTask(Unit unit, RaycastHit hit)
    {
        if (!hit.transform.TryGetComponent<Entity>(out var entity))
            return new IUnitTask[] { new MoveToVectorTask(unit, hit.point) };

        switch (entity)
        {
            case IInteractable:
                return new IUnitTask[]
                {
                    new MoveToEntityTask(unit, entity, (unit as IStatsable)[EntityStatsType.InteractRange].StatValue)
                    //TODO: задача с взаимодействием
                };
            case IHealthable e when e.EntityReference.EntityType != unit.EntityType:
                return new IUnitTask[]
                {
                    new MoveToEntityTask(unit, entity, (unit as IStatsable)[EntityStatsType.AttackRange].StatValue),
                    new AttackTask(unit as IAttacker, entity as IHealthable)
                };
            default:
                return new IUnitTask[]
                {
                    new MoveToEntityTask(unit, entity, (unit as IStatsable)[EntityStatsType.InteractRange].StatValue)
                };
        }
    }


    private void SelectAloneUnit()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0) || UIRayCast)
            return;

        if (!Physics.Raycast(direction, out RaycastHit hit))
            return;

        if (!hit.transform.TryGetComponent<IControllable>(out var unit))
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

        foreach (var unit in _entryPoint.Units.Where(u => u is IControllable))
        {
            if (!bounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position)))
                continue;
            _selectedUnits.Add(unit);
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
