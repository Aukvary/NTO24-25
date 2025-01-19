using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BearActivityManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private InventoryHUD _inventoryHUD;

    [SerializeField]
    private StorageHUD _storageHUD;
    [SerializeField]
    private Storage _storage;
    [SerializeField]
    private BreakeableObject _burovHome;

    [Header("Unit Selection")]
    [SerializeField]
    private Color _selectingAreaColor;

    private List<Unit> _allUnits = new();

    private Bear[] _bears;

    private List<Bear> _controlledUnits = new();


    private Vector3 _startMousePos;
    private Vector3 _endMousePos;
    private Ray direction => Camera.main.ScreenPointToRay(Input.mousePosition);

    public IEnumerable<Bear> Bears => _bears;

    public IEnumerable<Unit> AllUnits => _allUnits;

    public IEnumerable<Bear> SelectedUnits => _controlledUnits;

    public Unit InventoryUnit => _inventoryHUD.Unit;

    public event Action OnHotKeySelect;

    private void Awake()
    {
        _bears = _allUnits.Where(u => u is Bear).Cast<Bear>().ToArray();
    }

    private void Start()
    {
        _storageHUD.UpdateHUD(_storage);
        _storage.OnLayOutItems.AddListener(_storageHUD.UpdateHUD);
    }
    private void Update()
    {
        SetUnitTask();
        SelectUnitGroup();
        SelectAloneUnit();
    }

    public void AddUnit(Unit unit)
        => _allUnits.Add(unit);

    public bool RemoveUnit(Unit unit)
        => _allUnits.Remove(unit);
    private void SetUnitTask()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse1) || !_controlledUnits.Any() || Input.GetKey(KeyCode.LeftAlt))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (!Physics.Raycast(direction, out var actionHit))
            return;

        if (actionHit.transform.TryGetComponent<IInteractable>(out var obj))
        {
            if (obj is Bear)
                return;

            if ((object)obj == _burovHome)
                return;

            foreach (var bear in _controlledUnits)
                bear?.InteractWith(obj);

            return;
        }



        if (Physics.Raycast(direction, out var groundHit, LayerMask.GetMask("Ground")))
        {
            foreach (var unit in _controlledUnits)
            {
                try
                {
                    unit.MoveTo(groundHit.point);
                }
                catch { }
            }
        }
    }


    private void SelectAloneUnit()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.LeftShift))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!Physics.Raycast(direction, out RaycastHit hit))
            return;

        if (!hit.transform.TryGetComponent<Bear>(out var unit))
            return;

        _controlledUnits.Clear();

        _inventoryHUD.Unit = unit;
        _controlledUnits.Add(unit);
    }

    private void SelectUnitGroup()
    {
        if (_startMousePos == _endMousePos)
            return;
        Vector3 v1 = Camera.main.ScreenToViewportPoint(_startMousePos);
        Vector3 v2 = Camera.main.ScreenToViewportPoint(_endMousePos);

        Vector3 minOffset = Vector3.Min(v1, v2);
        Vector3 maxOffset = Vector3.Max(v1, v2);

        minOffset.z = Camera.main.nearClipPlane;
        maxOffset.z = Camera.main.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(minOffset, maxOffset);

        foreach (var unit in _bears)
        {
            if (!bounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position)))
                continue;
            _controlledUnits.Add(unit);
        }
    }

    public void HotKeySelectBear(Bear bear)
    {
        _controlledUnits.Clear();
        _controlledUnits.Add(bear);
        _inventoryHUD.Unit = bear;

        OnHotKeySelect?.Invoke();
    }

    private void OnGUI()
    {
        if (_startMousePos != _endMousePos && !Input.GetKey(KeyCode.Mouse0))
        {
            _startMousePos = _endMousePos = Vector3.zero;
            return;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _startMousePos = Input.mousePosition;
            _startMousePos.y = Screen.height - _startMousePos.y;

        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _endMousePos = Input.mousePosition;
            _endMousePos.y = Screen.height - _endMousePos.y;
        }

        var topLeft = Vector3.Min(_startMousePos, _endMousePos);
        var bottomRight = Vector3.Max(_startMousePos, _endMousePos);

        var rect = Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);


        Color areaColor = _selectingAreaColor;
        areaColor.a = 0.25f;
        UIUtils.DrawSelectArea(rect, areaColor, _selectingAreaColor, 2);
    }
}
