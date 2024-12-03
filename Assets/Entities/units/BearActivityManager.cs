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
    
    [Header("Unit Selection")]
    [SerializeField]
    private Color _selectingAreaColor;
    [SerializeField]
    private float _hotkeyChangeZCameraPosition;

    private List<Unit> _allUnits = new();

    private Unit[] _bears;

    private List<Unit> _controlledUnits = new();


    private Vector3 _startMousePos;
    private Vector3 _endMousePos;
    private Ray direction => Camera.main.ScreenPointToRay(Input.mousePosition);

    public IEnumerable<Unit> Bears => _bears;

    public IEnumerable<Unit> AllUnits => _allUnits;

    private void Awake()
    {
        _bears = _allUnits.Where(u => !u.IsBee).ToArray();
    }
    private void Start()
    {
        _storageHUD.UpdateHUD(_storage);
        _storage.OnLayOut += _storageHUD.UpdateHUD;

        transform.rotation = Quaternion.identity;
        transform.position = new(
            _bears[0].transform.position.x,
            transform.position.y,
            _bears[0].transform.position.z - _hotkeyChangeZCameraPosition);
    }
    private void Update()
    {
        SetUnitTask();
        SelectUnitGroup();
        SelectAloneUnit();
        HotKeySelect();
    }

    public void AddUnit(Unit unit)
        => _allUnits.Add(unit);

    public bool RemoveUnit(Unit unit) 
        => _allUnits.Remove(unit);
    private void SetUnitTask()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse1) || !_controlledUnits.Any())
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!Physics.Raycast(direction, out var actionHit))
            return;

        if (actionHit.transform.TryGetComponent<Unit>(out var bee) && bee.IsBee)
        {
            foreach (var bear in _controlledUnits)
                bear.Attack(bee);
            return;
        }

        actionHit.transform.TryGetComponent<ActionObject>(out var obj);

        switch (obj)
        {
            case ResourceObjectSpawner:
                foreach (var unit in _controlledUnits)
                    unit.Extract(obj as ResourceObjectSpawner);
                return;

            case Storage:
                foreach (var unit in _controlledUnits)
                    unit.LayOutItems(obj as Storage);
                return;

            case ConstructionObject:
                foreach (var unit in _controlledUnits)
                    unit.Build(obj as ConstructionObject);
                return;

            case BreakeableObject:
                foreach (var unit in _controlledUnits)
                    unit.Attack(obj as BreakeableObject);
                return;
        }

        if (actionHit.transform.TryGetComponent<Unit>(out var enemy) && enemy.IsBee)
            foreach (var unit in _controlledUnits)
                unit.Attack(enemy);

        if (Physics.Raycast(direction, out var groundHit, LayerMask.GetMask("Ground")))
        {
            foreach (var unit in _controlledUnits)
                unit.MoveTo(groundHit.point);
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
        if (!Input.GetKey(KeyCode.LeftControl))
            _controlledUnits.Clear();

        if (!hit.transform.TryGetComponent<Unit>(out var unit))
        {
            _inventoryHUD.Unit = null;
            return;
        }
        if (unit.IsBee)
            return;
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
            if (!bounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position)) || unit.IsBee)
                continue;
                _controlledUnits.Add(unit);
        }
    }

    private void HotKeySelect()
    {
        var hotkey = KeyCode.Alpha1;
        for (int i = 0; i < _bears.Length; i++)
        {
            if (Input.GetKeyDown(hotkey + i))
            {
                _controlledUnits.Clear();
                _controlledUnits.Add(_bears[i]);
                _inventoryHUD.Unit = _bears[i];

                transform.rotation = Quaternion.identity;
                transform.position = new(
                    _bears[i].transform.position.x, 
                    transform.position.y,
                    _bears[i].transform.position.z - _hotkeyChangeZCameraPosition);
            }
        }
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
